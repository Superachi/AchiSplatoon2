using Terraria.ID;
using Terraria;
using Terraria.Chat;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using log4net;
using Terraria.ModLoader;
using System.IO;
using NVorbis.Contracts;
using System;
using AchiSplatoon2.Content.Players;

namespace AchiSplatoon2.Helpers
{
    enum PacketType
    {
        TestRequest,
        TestResponse,
        PublicMessage,
        SpecialReadyDust,
    }

    internal static class NetHelper
    {
        public const bool EnableNetDebug = true;
        public static bool IsThisTheServer()
        {
            return Main.netMode == NetmodeID.Server;
        }

        public static bool IsThisAClient()
        {
            return Main.netMode == NetmodeID.MultiplayerClient;
        }

        public static ModPacket GetNewPacket()
        {
            return ModContent.GetInstance<AchiSplatoon2>().GetPacket();
        }

        public static void WritePacketID(ModPacket packet, int id)
        {
            packet.Write((byte)id);
        }

        public static void WritePacketIsFromServer(ModPacket packet)
        {
            packet.Write(IsThisTheServer());
        }

        public static void WritePacketFromWhoID(ModPacket packet, int fromWho)
        {
            packet.Write(fromWho);
        }

        public static void WritePacketMessage(ModPacket packet, string message)
        {
            packet.Write(message);
        }

        public static void SendPacket(ModPacket packet, int toClient = -1, int ignoreClient = -1)
        {
            if (EnableNetDebug && IsThisAClient())
            {
                Main.NewText($"Sending packet. My ID is {Main.LocalPlayer.whoAmI}.");
            }

            packet.Send(toClient: toClient, ignoreClient: ignoreClient);
        }

        public static bool DoesMethodCallerMatchLocalPlayer(Player caller)
        {
            return caller.whoAmI == Main.LocalPlayer.whoAmI;
        }

        private static Player GetPlayerFromPacket(int fromWho)
        {
            return Main.player[fromWho];
        }

        private static string GetPlayerNameFromPacket(int fromWho)
        {
            return Main.player[fromWho].name;
        }

        private static string GetPlayerNameAndIDFromPacket(int fromWho)
        {
            return $"{Main.player[fromWho].name} (#{fromWho})";
        }

        private static InkWeaponPlayer GetModPlayerFromPacket(int fromWho)
        {
            Player p = GetPlayerFromPacket(fromWho);
            return p.GetModPlayer<InkWeaponPlayer>();
        }

        #region Net code packet testing
        private static void SendTestPacket(int packetID, int fromWho, int toWho = -1)
        {
            ModPacket packet = GetNewPacket();
            WritePacketID(packet, packetID);
            WritePacketIsFromServer(packet);
            WritePacketFromWhoID(packet, fromWho);

            string message = "";
            string name = Main.player[fromWho].name;
            switch (packetID)
            {
                case 0:
                    message = $"This is {name} (ID {fromWho}), please respond.";
                    break;
                case 1:
                    message = $"{name} (ID {fromWho}) here, this is my response.";
                    break;
            }

            WritePacketMessage(packet, message);
            packet.Send(toClient: toWho, ignoreClient: fromWho);
        }

        public static void SendRequestTestPacket(int fromWho, int toWho = -1)
        {
            SendTestPacket(0, fromWho, toWho);
            Main.NewText($"Sent request test packet!");
        }

        public static void SendResponseTestPacket(int fromWho, int toWho = -1)
        {
            SendTestPacket(1, fromWho, toWho);
            Main.NewText($"Sent response test packet!");
        }

        public static void ReceiveRequestTestPacket(BinaryReader reader, int fromWho)
        {
            bool fromServer = reader.ReadBoolean();
            fromWho = reader.ReadInt32();
            string message = reader.ReadString();

            if (IsThisTheServer())
            {
                // If we are a server...
                // Forward this message to all the clients, except the original poster (fromWho)
                SendRequestTestPacket(fromWho);
            } else
            {
                // If we are a client...
                // Send a response to the server, who will forward it
                Main.NewText($"Received request test packet! (Message: '{message}'. From server: {fromServer})", ColorHelper.GetInkColor(InkColor.Blue));
                SendResponseTestPacket(Main.LocalPlayer.whoAmI, fromWho);
            }
        }

        public static void ReceiveResponseTestPacket(BinaryReader reader, int fromWho)
        {
            bool fromServer = reader.ReadBoolean();
            fromWho = reader.ReadInt32();
            string message = reader.ReadString();

            if (IsThisTheServer())
            {
                // If we are a server...
                // Forward this message to all the clients, except the original poster (fromWho)
                SendResponseTestPacket(fromWho);
            }
            else
            {
                // If we are a client...
                // We're done :) We received a response to the client we sent a request to.
                Main.NewText($"Received response test packet! (Message: '{message}'. From server: {fromServer})", ColorHelper.GetInkColor(InkColor.Green));
            }
        }
        #endregion

        #region Chat functions
        public static void SendPublicMessage(int fromWho, string message, bool appendName = true)
        {
            ModPacket packet = GetNewPacket();
            WritePacketID(packet, 2);
            WritePacketFromWhoID(packet, fromWho);

            string name = Main.player[fromWho].name;
            string finalMessage = message;
            if (appendName)
            {
                finalMessage = $"<{name} (#{fromWho})> {message}";
            }

            WritePacketMessage(packet, finalMessage);
            packet.Send(toClient: -1, ignoreClient: fromWho);
        }

        public static string ReceiveMessage(BinaryReader reader, int fromWho)
        {
            fromWho = reader.ReadInt32();
            string message = reader.ReadString();

            if (IsThisTheServer())
            {
                // Forward the message
                SendPublicMessage(fromWho, message, false);
            }
            else
            {
                // Display the message (client-side)
                Main.NewText($"{message}", ColorHelper.GetInkColor(InkColor.Order));
            }

            return message;
        }

        #endregion

        public static void SendPlayerIsSpecialReady(int fromWho, bool isSpecialReady)
        {
            // Prepare data
            Player player = Main.LocalPlayer;
            ModPacket packet = GetNewPacket();

            // Write
            WritePacketID(packet, (int)PacketType.SpecialReadyDust);
            WritePacketFromWhoID(packet, fromWho);
            packet.Write(isSpecialReady);

            // Send
            SendPacket(packet, toClient: -1, ignoreClient: fromWho);
        }

        public static void ReceivePlayerIsSpecialReady(BinaryReader reader, int fromWho)
        {
            // Read data
            fromWho = reader.ReadInt32();
            bool isSpecialReady = reader.ReadBoolean();

            // Respond
            if (IsThisTheServer())
            {
                // Forward
                SendPlayerIsSpecialReady(fromWho, isSpecialReady);
            }
            else
            {
                var modPlayer = GetModPlayerFromPacket(fromWho);
                modPlayer.SpecialReady = isSpecialReady;
            }
        }
    }
}
