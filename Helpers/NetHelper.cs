using Terraria.ID;
using Terraria;
using Terraria.Chat;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using log4net;

namespace AchiSplatoon2.Helpers
{
    internal static class NetHelper
    {
        public static bool IsThisTheServer()
        {
            return Main.netMode == NetmodeID.Server;
        }

        public static void BroadcastAndLogMessage(string message, ILog logger)
        {
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), new Color(255, 255, 255));
            logger.Info(message);
        }
    }
}
