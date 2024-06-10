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
        public static Color GenerateInkColor(InkColor enumVal)
        {
            Color finalColor;
            switch (enumVal) {
                case InkColor.Orange:
                    finalColor = Color.Lerp(Color.Crimson, Color.Orange, Main.rand.NextFloat() * 0.5f);
                    break;
                case InkColor.Pink:
                    finalColor = Color.Lerp(Color.MediumPurple, Color.Magenta, Main.rand.NextFloat() * 0.5f);
                    break;
                case InkColor.Green:
                    finalColor = Color.Lerp(Color.SeaGreen, Color.LimeGreen, Main.rand.NextFloat() * 0.5f);
                    break;
                case InkColor.Yellow:
                    finalColor = Color.Lerp(Color.Orange, Color.Yellow, Main.rand.NextFloat() * 0.5f);
                    break;
                case InkColor.Red:
                    finalColor = Color.Lerp(Color.Crimson, Color.HotPink, Main.rand.NextFloat() * 0.5f);
                    break;
                default:
                    finalColor = Color.Lerp(Color.Aqua, Color.Blue, Main.rand.NextFloat() * 0.5f);
                    break;
            }
            return finalColor;
        }
    }
}
