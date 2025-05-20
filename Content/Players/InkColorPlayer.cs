using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.Content.Projectiles.ProjectileVisuals;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.ModConfigs;
using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AchiSplatoon2.Content.Players
{
    internal class InkColorPlayer : ModPlayer
    {
        public Color colorA = Color.White;
        public Color colorB = Color.White;

        public bool useColorsFromChips = true;
        public bool useRainbowColors = false;
        public bool useConfigColors = false;

        public enum IncrementType
        {
            Static,
            AttackBased,
            TimeBased,
            HealthBased,
        }

        public enum InkColorType
        {
            Single,
            Dual,
            ColorChips,
            Rainbow,
            Config,
            Void
        }

        public InkColorType inkColorType = InkColorType.Single;
        public IncrementType incrementType = IncrementType.Static;

        public Color GetCurrentColor()
        {
            if (Player.HeldItem.ModItem != null && Player.HeldItem.ModItem.GetType().GetCustomAttribute<ShimmerOrderWeaponAttribute>() != null)
            {
                return ColorHelper.LerpBetweenColorsPerfect(Main.DiscoColor, Color.White, 0.7f);
            }

            switch(inkColorType)
            {
                case InkColorType.Rainbow:
                    return ColorHelper.LerpBetweenColorsPerfect(Main.DiscoColor, Color.White, 0.2f);

                case InkColorType.Config:
                    var tempColor = ModContent.GetInstance<ClientConfig>().CustomInkColor;
                    return ColorHelper.LerpBetweenColorsPerfect(tempColor, Color.White, 0.05f);

                case InkColorType.ColorChips:
                    var color = Player.GetModPlayer<ColorChipPlayer>().GetColorResultingFromChips();
                    return ColorHelper.LerpBetweenColorsPerfect(color, Color.White, 0.05f);

                case InkColorType.Dual:
                    // See logic below
                    break;
            }

            int wrapValue = 20;
            var attacksUsed = Player.GetModPlayer<StatisticsPlayer>().attacksUsed;
            bool addAttack = (attacksUsed / wrapValue) % 2 == 0;
            int attackValue = addAttack ? attacksUsed % wrapValue : wrapValue - attacksUsed % wrapValue - 1;

            var lerpAmount = (attackValue % wrapValue) / (float)wrapValue;
            return ColorHelper.LerpBetweenColorsPerfect(colorA, colorB, lerpAmount);
        }

        #region Set color(s)

        public void SetSingleColor(Color colorA)
        {
            inkColorType = InkColorType.Single;
            incrementType = IncrementType.Static;

            this.colorA = colorA;

            SetOverheadText("Ink color updated!");
        }

        public void SetDualColor(IncrementType type, Color colorA, Color? colorB = null)
        {
            inkColorType = InkColorType.Dual;
            incrementType = type;

            this.colorA = colorA;
            this.colorB = colorB ?? colorA;

            SetOverheadText("Ink color updated!");
        }

        public void ApplyConfigColors()
        {
            inkColorType = InkColorType.Config;
            incrementType = IncrementType.Static;
            SetOverheadText("Now using the color from your settings!");
        }

        public void ApplyChipColors()
        {
            inkColorType = InkColorType.ColorChips;
            incrementType = IncrementType.Static;
            SetOverheadText("Now using colors based on the color chips you carry!");
        }

        public void ApplyRainbowColors()
        {
            inkColorType = InkColorType.Rainbow;
            incrementType = IncrementType.TimeBased;
            SetOverheadText("Now using rainbow colors!");
        }

        #endregion

        private void SetOverheadText(string text)
        {
            Player.GetModPlayer<HudPlayer>().SetOverheadText(
                text,
                displayTime: text.Length * 4,
                color: ColorHelper.LerpBetweenColorsPerfect(ColorHelper.ColorWithAlpha255(GetCurrentColor()), Color.White, 0.2f));
        }

        #region Save/load

        public override void SaveData(TagCompound tag)
        {
            tag[$"{nameof(colorA)}"] = colorA;
            tag[$"{nameof(colorB)}"] = colorB;

            tag[$"{nameof(inkColorType)}"] = inkColorType.ToString();
            tag[$"{nameof(incrementType)}"] = incrementType.ToString();
        }

        public override void LoadData(TagCompound tag)
        {
            colorA = tag.Get<Color>($"{nameof(colorA)}");
            colorB = tag.Get<Color>($"{nameof(colorB)}");

            if (Enum.TryParse(tag.GetString($"{nameof(inkColorType)}"), out InkColorType newInkColorType))
            {
                inkColorType = newInkColorType;
            }

            if (Enum.TryParse(tag.GetString($"{nameof(incrementType)}"), out IncrementType newIncrementType))
            {
                incrementType = newIncrementType;
            }
        }

        #endregion
    }
}
