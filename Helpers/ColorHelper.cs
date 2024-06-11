using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Color = Microsoft.Xna.Framework.Color;

namespace AchiSplatoon2.Helpers
{
    public enum InkColor
    {
        Blue,
        Orange,
        Pink,
        Green,
        Yellow,
        Red,
    }

    internal static class ColorHelper
    {
        public static InkColor GetRandomInkColor()
        {
            return (InkColor)Main.rand.Next(0, 6);
        }

        public static Color GenerateInkColor(InkColor enumVal)
        {
            Color finalColor;
            switch (enumVal) {
                case InkColor.Orange:
                    finalColor = Color.Lerp(new Color(255, 93, 82), new Color(255, 190, 79), Main.rand.NextFloat());
                    break;
                case InkColor.Pink:
                    finalColor = Color.Lerp(new Color(255, 107, 169), new Color(210, 74, 255), Main.rand.NextFloat());
                    break;
                case InkColor.Green:
                    finalColor = Color.Lerp(new Color(75, 219, 101), new Color(23, 212, 155), Main.rand.NextFloat());
                    break;
                case InkColor.Yellow:
                    finalColor = Color.Lerp(new Color(255, 190, 59), new Color(255, 228, 122), Main.rand.NextFloat());
                    break;
                default:
                    finalColor = Color.Lerp(new Color(64, 118, 255), new Color(64, 223, 255), Main.rand.NextFloat());
                    break;
            }
            return finalColor;
        }
    }
}
