using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Helpers
{
    internal class ChatHelper
    {
        public static void SendChatToThisClient(string message, Color? color = null)
        {
            Color textColor = color ??= Color.White;
            Main.chatMonitor.NewText(message, textColor.R, textColor.G, textColor.B);
        }
    }
}
