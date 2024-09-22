using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AchiSplatoon2.Content.Players
{
    internal class InkColorPlayer : BaseModPlayer
    {
        public Color currentColor = Color.Red;
        public float currentHue = 0;

        public Color IncreaseHueBy(float value)
        {
            currentColor = ColorHelper.IncreaseHueBy(value, currentColor);
            currentHue = ColorHelper.GetHueFromColor(currentColor);

            if (currentHue > 225 && currentHue <= 240)
            {
                var color = ColorHelper.LerpBetweenColorsPerfect(ColorHelper.IncreaseHueBy(value, currentColor), Color.Aqua, 0.3f);
                return color;
            }

            if (currentHue > 240 && currentHue <= 255)
            {
                var color = ColorHelper.LerpBetweenColorsPerfect(ColorHelper.IncreaseHueBy(value, currentColor), Color.Aqua, 0.1f);
                return color;
            }

            return currentColor;
        }
    }
}
