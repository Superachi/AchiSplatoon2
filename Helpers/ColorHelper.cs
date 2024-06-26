using Terraria;
using Color = Microsoft.Xna.Framework.Color;

namespace AchiSplatoon2.Helpers
{
    public enum InkColor
    {
        Red,
        Blue,
        Yellow,
        Purple,
        Green,
        Aqua,
        Order,
    }

    internal static class ColorHelper
    {
        public static InkColor GetRandomInkColor()
        {
            return (InkColor)Main.rand.Next(0, 6);
        }

        public static string TextWithNeutralColor(string input)
        {
            return $"[c/ffffff:{input}]";
        }

        public static string TextWithFlavorColor(string input)
        {
            return $"[c/64aaff:{input}]";
        }

        public static string TextWithFlavorColorAndQuotes(string input)
        {
            return $"[c/64aaff:\"{input}\"]";
        }

        public static string TextWithFunctionalColor(string input)
        {
            return $"[c/ff8e2c:{input}]";
        }

        public static Color GetInkColor(InkColor enumVal)
        {
            Color finalColor;
            switch (enumVal)
            {
                case InkColor.Blue:
                    finalColor = new Color(0, 103, 255);
                    break;
                case InkColor.Yellow:
                    finalColor = new Color(255, 198, 0);
                    break;
                case InkColor.Purple:
                    finalColor = new Color(185, 0, 255);
                    break;
                case InkColor.Green:
                    finalColor = new Color(78, 255, 43);
                    break;
                case InkColor.Aqua:
                    finalColor = new Color(0, 255, 238);
                    break;
                case InkColor.Red:
                    finalColor = new Color(255, 41, 0);
                    break;
                case InkColor.Order:
                default:
                    finalColor = new Color(228, 203, 178);
                    break;
            }
            return finalColor;
        }

        public static Color LerpBetweenInkColors(InkColor primaryColor, InkColor secondaryColor, float amount)
        {
            // Exceptions to make certain blends look nicer
            // Red & Blue
            if (primaryColor == InkColor.Red && secondaryColor == InkColor.Blue)
            {
                return new Color(199, 65, 228);
            }

            if (primaryColor == InkColor.Blue && secondaryColor == InkColor.Red)
            {
                return new Color(150, 88, 255);
            }

            // Red & Aqua
            if (primaryColor == InkColor.Red && secondaryColor == InkColor.Aqua)
            {
                return new Color(255, 143, 248);
            }

            if (primaryColor == InkColor.Aqua && secondaryColor == InkColor.Red)
            {
                return new Color(196, 143, 255);
            }


            // Red & Green
            if (primaryColor == InkColor.Red && secondaryColor == InkColor.Green)
            {
                return new Color(244, 210, 77);
            }

            if (primaryColor == InkColor.Green && secondaryColor == InkColor.Red)
            {
                return new Color(211, 228, 65);
            }

            // Blue & Yellow
            if (primaryColor == InkColor.Blue && secondaryColor == InkColor.Yellow)
            {
                return new Color(72, 229, 64);
            }

            if (primaryColor == InkColor.Yellow && secondaryColor == InkColor.Blue)
            {
                return new Color(107, 231, 61);
            }

            // Purple & Green
            if (primaryColor == InkColor.Purple && secondaryColor == InkColor.Green)
            {
                return new Color(44, 167, 255);
            }

            if (primaryColor == InkColor.Green && secondaryColor == InkColor.Purple)
            {
                return new Color(0, 203, 184);
            }

            return LerpBetweenColors(GetInkColor(primaryColor), GetInkColor(secondaryColor), amount);
        }

        public static Color LerpBetweenColors(Color primaryColor, Color secondaryColor, float amount)
        {
            return Color.Lerp(primaryColor, secondaryColor, amount + Main.rand.NextFloat(-0.1f, 0.1f));
        }
    }
}
