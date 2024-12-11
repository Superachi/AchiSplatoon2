using AchiSplatoon2.Content.Items.Weapons.Test;
using AchiSplatoon2.Content.Projectiles.Debug;
using AchiSplatoon2.Helpers;
using log4net;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using static AchiSplatoon2.Helpers.NetHelper;

namespace AchiSplatoon2.Netcode
{
    enum PacketType
    {
        TestRequest,
        TestResponse,
        PublicMessage,
        PingRequest,
        PingResponse,
    }

    internal class GenericPacketHandler
    {
        private readonly BinaryReader _reader;
        private readonly int _whoAmI;
        private readonly ILog _logger;

        public GenericPacketHandler(BinaryReader reader, int whoAmI, ILog logger)
        {
            _reader = reader;
            _whoAmI = whoAmI;
            _logger = logger;
        }

        public void HandlePacket()
        {
            byte msgType = _reader.ReadByte();

            switch (msgType)
            {
                case (int)PacketType.TestRequest:
                    ReceiveRequestTestPacket(_reader, _whoAmI);
                    _logger.Info($"Received test request packet");
                    break;
                case (int)PacketType.TestResponse:
                    ReceiveResponseTestPacket(_reader, _whoAmI);
                    _logger.Info($"Received test response packet");
                    break;
                case (int)PacketType.PublicMessage:
                    string message = ReceiveMessage(_reader, _whoAmI);
                    break;
                case (int)PacketType.PingRequest:
                    ServerRespondToPingPacket(_reader, _whoAmI);
                    break;
                case (int)PacketType.PingResponse:
                    ClientRespondToServerPingPacket();
                    break;
                default:
                    _logger.WarnFormat("Unknown Message type: {0}", msgType);
                    break;
            }
        }

        #region Net code packet testing

        private static void SendTestPacket(int packetID, int fromWho, int toWho = -1)
        {
            ModPacket packet = GetNewPacket();
            WritePacketHandlerType(packet, (int)PacketHandlerType.Generic);
            WritePacketType(packet, packetID);
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
            SendPacket(packet, toClient: toWho, ignoreClient: fromWho);
        }

        public static void SendRequestTestPacket(int fromWho, int toWho = -1)
        {
            if (IsSinglePlayer()) { return; }

            SendTestPacket(0, fromWho, toWho);
            Main.NewText($"Sent request test packet!");
        }

        public static void SendResponseTestPacket(int fromWho, int toWho = -1)
        {
            if (IsSinglePlayer()) { return; }

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
            }
            else
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

        private static void ServerRespondToPingPacket(BinaryReader reader, int fromWho)
        {
            int originalSender = reader.ReadInt32();

            if (IsThisTheServer())
            {
                // Return with a new packet back to sender
                ModPacket packet = GetNewPacket();
                WritePacketHandlerType(packet, (int)PacketHandlerType.Generic);
                WritePacketType(packet, (int)PacketType.PingResponse);

                SendPacket(packet, toClient: originalSender);
            }
        }

        private static void ClientRespondToServerPingPacket()
        {
            if (IsThisTheServer()) { return; }

            if (Main.LocalPlayer.ownedProjectileCounts[ModContent.ProjectileType<NetcodeInspectorProjectile>()] != 0)
            {
                foreach (var projectile in Main.ActiveProjectiles)
                {
                    if (projectile.ModProjectile is NetcodeInspectorProjectile
                        && projectile.owner == Main.LocalPlayer.whoAmI)
                    {
                        var p = projectile.ModProjectile as NetcodeInspectorProjectile;
                        p!.ReceiveServerPing();
                    }
                }
            }
            else
            {
                DebugHelper.PrintInfo($"Received ping reponse from server. Use {nameof(NetcodeInspector)} item for more netcode debugging info.");
            }
        }

        #endregion

        #region Chat functions

        public static void SendPublicMessage(int fromWho, string message, bool appendName = true)
        {
            if (IsSinglePlayer()) { return; }

            ModPacket packet = GetNewPacket();
            WritePacketHandlerType(packet, (int)PacketHandlerType.Generic);
            WritePacketType(packet, 2);
            WritePacketFromWhoID(packet, fromWho);

            string name = Main.player[fromWho].name;
            string finalMessage = message;
            if (appendName)
            {
                finalMessage = $"<{name} (#{fromWho})> {message}";
            }

            WritePacketMessage(packet, finalMessage);
            SendPacket(packet, toClient: -1, ignoreClient: fromWho);
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
    }
}
