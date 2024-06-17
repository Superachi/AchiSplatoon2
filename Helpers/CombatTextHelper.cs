using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Helpers
{
    internal static class CombatTextHelper
    {
        public static void DisplayText(string text, Vector2 position, Color? color = null, int rectWidth = 20, int rectHeight = 20)
        {
            DisplayText(text, (int)position.X, (int)position.Y, color, rectWidth, rectHeight);
        }

        public static void DisplayText(string text, int x, int y, Color? color = null, int rectWidth = 20, int rectHeight = 20)
        {
            if (color == null)
            {
                color = Color.White;
            }

            var rect = new Rectangle(x - rectWidth / 2, y - rectHeight / 2, rectWidth, rectHeight);
            CombatText.NewText(rect, (Color)color, text);
        }
    }
}
