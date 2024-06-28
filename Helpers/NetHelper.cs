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

        public static void WritePacketHandlerType(ModPacket packet, int id)
        {
            packet.Write((byte)id);
        }

        public static void WritePacketType(ModPacket packet, int id)
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

        public static Player GetPlayerFromPacket(int fromWho)
        {
            return Main.player[fromWho];
        }

        public static string GetPlayerNameFromPacket(int fromWho)
        {
            return Main.player[fromWho].name;
        }

        public static string GetPlayerNameAndIDFromPacket(int fromWho)
        {
            return $"{Main.player[fromWho].name} (#{fromWho})";
        }

        public static InkWeaponPlayer GetModPlayerFromPacket(int fromWho)
        {
            Player p = GetPlayerFromPacket(fromWho);
            return p.GetModPlayer<InkWeaponPlayer>();
        }
    }
}
