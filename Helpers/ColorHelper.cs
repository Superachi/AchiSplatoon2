using System;
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

    public static class ColorHelper
    {
        public static InkColor GetRandomInkColor() => (InkColor)Main.rand.Next(0, 6);

        private static string TextWithColor(string input, string rgb)
        {
            return $"[c/{rgb}:{input}]";
        }

        public static string TextWithNeutralColor(string input) => TextWithColor(input, "ffffff");
        public static string TextWithFlavorColor(string input) => TextWithColor(input, "a69e9a");
        public static string TextWithFlavorColorAndQuotes(string input) => TextWithColor($"\"{input}\"", "a69e9a");
        public static string TextWithFunctionalColor(string input) => TextWithColor(input, "ff8e2c");
        public static string TextWithMainWeaponColor(string input) => TextWithColor(input, "f23b55");
        public static string TextWithSubWeaponColor(string input) => TextWithColor(input, "3479de");
        public static string TextWithSpecialWeaponColor(string input) => TextWithColor(input, "7d45ff");
        public static string TextWithBonusColor(string input) => TextWithColor(input, "63a864");
        public static string TextWithErrorColor(string input) => TextWithColor(input, "f23b55");
        public static string TextWithPearlColor(string input) => TextWithColor(input, "eea39b");

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
                    finalColor = new Color(128, 255, 0);
                    break;
                case InkColor.Aqua:
                    finalColor = new Color(0, 255, 190);
                    break;
                case InkColor.Red:
                    finalColor = new Color(255, 0, 40);
                    break;
                case InkColor.Order:
                default:
                    finalColor = new Color(228, 203, 178);
                    break;
            }

            return finalColor;
        }

        public static Color LerpBetweenColors(Color primaryColor, Color secondaryColor, float amount)
        {
            return Color.Lerp(primaryColor, secondaryColor, amount + Main.rand.NextFloat(-0.1f, 0.1f));
        }

        public static Color LerpBetweenColorsPerfect(Color primaryColor, Color secondaryColor, float amount)
        {
            return Color.Lerp(primaryColor, secondaryColor, amount);
        }

        public static Color CombinePrimarySecondaryColors(Color primaryColor, Color secondaryColor, Color? colorBias = null)
        {
            RgbToHsv(primaryColor.R, primaryColor.G, primaryColor.B, out float hueA, out float satA, out float valA);
            RgbToHsv(secondaryColor.R, secondaryColor.G, secondaryColor.B, out float hueB, out float satB, out float valB);

            float middleHue = GetMiddleHue(hueA, hueB);
            if (colorBias != null)
            {
                middleHue = GetMiddleHue(middleHue, GetHueFromColor((Color)colorBias));
            }

            HsvToRgb(middleHue, (satA + satB) / 2, (valA + valB) / 2, out float r, out float g, out float b);
            return new Color(r / 255, g / 255, b / 255, 0);
        }

        public static string GetChipTextColor(InkColor inkColor)
        {
            var tempColor = GetInkColor(inkColor);
            var c = LerpBetweenColorsPerfect(tempColor, Color.White, 0.25f);
            return $"c/{c.R:X2}{c.G:X2}{c.B:X2}:";
        }

        public static Color ColorWithAlphaZero(Color input)
        {
            return new Color(input.R, input.G, input.B, 0);
        }

        public static Color ColorWithAlpha255(Color input)
        {
            return new Color(input.R, input.G, input.B, 255);
        }

        public static string ColorToString(Color input)
        {
            return $"{input.R}-{input.G}-{input.B}-{input.A}";
        }

        public static Color? StringToColor(string input)
        {
            var subStrings = input.Split('-');
            int[] colorValues = new int[4];
            int i = 0;

            foreach (var subString in subStrings)
            {
                if (int.TryParse(subString, out int colorValue))
                {
                    colorValues[i] = colorValue;
                }
                else
                {
                    DebugHelper.PrintError($"Failed to parse color value from string: {input}");
                    return null;
                }

                i++;
            }

            return new Color(colorValues[0], colorValues[1], colorValues[2], colorValues[3]);
        }

        public static Color AddRandomHue(float hueVariance, Color colorInput)
        {
            return IncreaseHueBy(Main.rand.NextFloat(-hueVariance, hueVariance), colorInput);
        }

        // Source: https://stackoverflow.com/questions/11441055/how-to-change-color-hue-in-xna
        public static Color IncreaseHueBy(this Color colorInput, float value)
        {
            return IncreaseHueBy(value, colorInput);
        }

        public static Color IncreaseHueBy(float value, Color colorInput)
        {
            float h, s, v;

            RgbToHsv(colorInput.R, colorInput.G, colorInput.B, out h, out s, out v);
            h += value;

            float r, g, b;

            HsvToRgb(h, s, v, out r, out g, out b);

            colorInput.R = (byte)(r);
            colorInput.G = (byte)(g);
            colorInput.B = (byte)(b);

            return colorInput;
        }

        public static float GetHueFromColor(Color colorInput)
        {
            float h, s, v;

            RgbToHsv(colorInput.R, colorInput.G, colorInput.B, out h, out s, out v);
            return h;
        }

        private static readonly float hueMax = 360f;

        public static float GetMiddleHue(float hueA, float hueB)
        {
            if (Math.Abs(hueA - hueB) <= 180f)
            {
                return (hueA + hueB) / 2f;
            }

            if (hueA > hueB)
            {
                hueB += hueMax;
            }
            else
            {
                hueA += hueMax;
            }

            return (hueA + hueB) / 2f % hueMax;
        }

        public static Color HexToColor(string hex)
        {
            System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(hex);
            return new Color(color.R, color.G, color.B);
        }

        public static void RgbToHsv(float r, float g, float b, out float h, out float s, out float v)
        {
            float min, max, delta;
            min = System.Math.Min(System.Math.Min(r, g), b);
            max = System.Math.Max(System.Math.Max(r, g), b);
            v = max;               // v
            delta = max - min;
            if (max != 0)
            {
                s = delta / max;       // s

                if (r == max)
                    h = (g - b) / delta;       // between yellow & magenta
                else if (g == max)
                    h = 2 + (b - r) / delta;   // between cyan & yellow
                else
                    h = 4 + (r - g) / delta;   // between magenta & cyan
                h *= 60;               // degrees
                if (h < 0)
                    h += hueMax;
            }
            else
            {
                // r = g = b = 0       // s = 0, v is undefined
                s = 0;
                h = -1;
            }

        }

        public static void HsvToRgb(float h, float s, float v, out float r, out float g, out float b)
        {
            // Keeps h from going over 360
            h = h - ((int)(h / hueMax) * hueMax);

            int i;
            float f, p, q, t;
            if (s == 0)
            {
                // achromatic (grey)
                r = g = b = v;
                return;
            }
            h /= 60;           // sector 0 to 5

            i = (int)h;
            f = h - i;         // factorial part of h
            p = v * (1 - s);
            q = v * (1 - s * f);
            t = v * (1 - s * (1 - f));
            switch (i)
            {
                case 0:
                    r = v;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = v;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = v;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = v;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = v;
                    break;
                default:       // case 5:
                    r = v;
                    g = p;
                    b = q;
                    break;
            }
        }
    }
}
