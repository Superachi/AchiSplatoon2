using AchiSplatoon2.Content.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Helpers
{
    internal static class NetHelper
    {
        public const bool EnableNetDebug = true;

        /// <summary>
        /// Returns true if this instance of the game is the server.
        /// </summary>
        public static bool IsThisTheServer()
        {
            return Main.netMode == NetmodeID.Server;
        }

        /// <summary>
        /// Returns true if this instance of the game is a client, connected to the server.
        /// </summary>
        public static bool IsThisAClient()
        {
            return Main.netMode == NetmodeID.MultiplayerClient;
        }

        /// <summary>
        /// Returns true if this session is local singleplayer. Use this to prevent packets from being created or sent. Failure to do so will cause a silent exception.
        /// </summary>
        public static bool IsSinglePlayer()
        {
            return Main.netMode == NetmodeID.SinglePlayer;
        }

        /// <summary>
        /// Returns a network packet that can be written to.
        /// </summary>
        public static ModPacket GetNewPacket()
        {
            return ModContent.GetInstance<AchiSplatoon2>().GetPacket();
        }

        /// <summary>
        /// Determines which handler class should process this packet.
        /// </summary>
        public static void WritePacketHandlerType(ModPacket packet, int id)
        {
            packet.Write((byte)id);
        }

        /// <summary>
        /// Determines which action should be taken by the handler class to process this packet.
        /// </summary>
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
            if (IsSinglePlayer()) { return; }
            packet.Send(toClient: toClient, ignoreClient: ignoreClient);
        }

        /// <summary>
        /// Useful for functions that run on all clients. This prevents code on other clients from running on this client if their player ID does not match the local player ID.
        /// </summary>
        public static bool IsPlayerSameAsLocalPlayer(Player remotePlayer)
        {
            return remotePlayer.whoAmI == Main.LocalPlayer.whoAmI;
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
