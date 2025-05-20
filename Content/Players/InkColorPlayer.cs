using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Items.Accessories.InkTanks;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.ModConfigs;
using Microsoft.Xna.Framework;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

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

        public IncrementType incrementType = IncrementType.Static;
        public bool wrapColorValue = true;

        public void SetSingleColor(Color colorA)
        {
            DisableConfigColors();
            DisableChipColors();

            incrementType = IncrementType.Static;
            this.colorA = colorA;

            SetOverheadText("Ink color updated!");
        }

        public void SetDualColor(IncrementType type, Color colorA, Color? colorB = null)
        {
            DisableConfigColors();
            DisableChipColors();

            incrementType = type;
            this.colorA = colorA;
            this.colorB = colorB ?? colorA;

            SetOverheadText("Ink color updated!");
        }

        public Color GetCurrentColor()
        {
            if (Player.HasAccessory<EmpressInkTank>())
            {
                return ColorHelper.LerpBetweenColorsPerfect(Main.DiscoColor, Color.White, 0.2f);
            }

            if (Player.HeldItem.ModItem != null && Player.HeldItem.ModItem.GetType().GetCustomAttribute<ShimmerOrderWeaponAttribute>() != null)
            {
                return ColorHelper.LerpBetweenColorsPerfect(Main.DiscoColor, Color.White, 0.7f);
            }

            if (useConfigColors)
            {
                var tempColor = ModContent.GetInstance<ClientConfig>().CustomInkColor;
                return ColorHelper.LerpBetweenColorsPerfect(tempColor, Color.White, 0.05f);
            }

            if (useColorsFromChips)
            {
                var color = Player.GetModPlayer<ColorChipPlayer>().GetColorResultingFromChips();
                return ColorHelper.LerpBetweenColorsPerfect(color, Color.White, 0.05f);
            }

            return colorA;
        }

        private void SetOverheadText(string text)
        {
            Player.GetModPlayer<HudPlayer>().SetOverheadText(
                text,
                displayTime: text.Length * 4,
                color: ColorHelper.LerpBetweenColorsPerfect(ColorHelper.ColorWithAlpha255(GetCurrentColor()), Color.White, 0.2f));
        }

        public bool ToggleConfigColors()
        {
            DisableChipColors();

            useConfigColors = !useConfigColors;

            if (useConfigColors)
            {
                SetOverheadText("Now using colors from your settings!");
            }
            else
            {
                SetOverheadText("Stopped using colors from your settings.");
            }

            return useConfigColors;
        }

        public void DisableConfigColors()
        {
            useConfigColors = false;
        }

        public bool ToggleChipColors()
        {
            DisableConfigColors();

            useColorsFromChips = !useColorsFromChips;

            if (useColorsFromChips)
            {
                SetOverheadText("Now using colors based on the two most common color chips you carry!");
            }
            else
            {
                SetOverheadText("Stopped using colors based on your color chips.");
            }

            return useColorsFromChips;
        }

        public void DisableChipColors()
        {
            useColorsFromChips = false;
        }
    }
}
