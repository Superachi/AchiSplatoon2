using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Items.Consumables.ColorVials.SingleColors;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.ModConfigs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AchiSplatoon2.Content.Players
{
    internal class InkColorPlayer : ModPlayer
    {
        public Color colorA = Color.White;
        public Color colorB = Color.White;

        public InkColorType inkColorType = InkColorType.Single;
        public IncrementType incrementType = IncrementType.Static;

        public bool initialColorSet = false;

        public enum IncrementType
        {
            Static,
            AttackBased,
            TimeBased,
            StatBased,
        }

        public enum InkColorType
        {
            Single,
            Dual,
            ColorChips,
            Rainbow,
            Config,
            Void,
            Biome,
            LifePercentage,
            InkPercentage
        }

        public override void PreUpdate()
        {
            if (!initialColorSet)
            {
                initialColorSet = true;

                var colorOptions = new List<Color>()
                {
                    new OrangeVial().ColorToSet,
                    new BlueVial().ColorToSet,
                    new PinkVial().ColorToSet,
                    new GreenVial().ColorToSet,
                };

                colorA = Main.rand.NextFromCollection(colorOptions);
                colorB = colorA;
            }
        }

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

                case InkColorType.Single:
                    return colorA;

                case InkColorType.Dual:
                    // See logic below
                    break;

                case InkColorType.LifePercentage:
                    float lifePercentage = Player.statLife / (float)Player.statLifeMax2;
                    return ColorHelper.LerpBetweenColorsPerfect(ColorHelper.HexToColor("9233ff"), ColorHelper.HexToColor("ff334e"), lifePercentage);

                case InkColorType.InkPercentage:
                    var inkPlayer = Player.GetModPlayer<InkTankPlayer>();
                    float inkPercentage = inkPlayer.InkAmount / (float)inkPlayer.InkAmountFinalMax;
                    return ColorHelper.LerpBetweenColorsPerfect(ColorHelper.HexToColor("3344ff"), ColorHelper.HexToColor("33ffdd"), inkPercentage);
                
                case InkColorType.Biome:
                    var player = Main.LocalPlayer;

                    var biomeColorResult = ColorHelper.HexToColor("4CD964");
                    if (player.ZoneNormalUnderground)
                    {
                        biomeColorResult = ColorHelper.HexToColor("ee6f2f");
                    }
                    else if (player.ZoneNormalCaverns)
                    {
                        biomeColorResult = ColorHelper.HexToColor("3553e9");
                    }

                    if (player.ZoneDesert)
                    {
                        biomeColorResult = ColorHelper.HexToColor("f3ba3c");
                    }
                    else if (player.ZoneBeach)
                    {
                        biomeColorResult = ColorHelper.HexToColor("51daef");
                    }
                    else if (player.ZoneJungle)
                    {
                        biomeColorResult = ColorHelper.HexToColor("bad74f");
                    }
                    else if (player.ZoneDungeon)
                    {
                        biomeColorResult = ColorHelper.HexToColor("2156ff");
                    }
                    else if (player.ZoneCorrupt)
                    {
                        biomeColorResult = ColorHelper.HexToColor("6f70ff");
                        if (Condition.Hardmode.IsMet())
                        {
                            biomeColorResult = ColorHelper.HexToColor("8cff00");
                        }
                    }
                    else if (player.ZoneCrimson)
                    {
                        biomeColorResult = ColorHelper.HexToColor("ff2636");
                        if (Condition.Hardmode.IsMet())
                        {
                            biomeColorResult = ColorHelper.HexToColor("ffd966");
                        }
                    }
                    else if (player.ZoneGlowshroom)
                    {
                        biomeColorResult = ColorHelper.HexToColor("3377ff");
                    }
                    else if (player.ZoneHallow)
                    {
                        biomeColorResult = ColorHelper.HexToColor("58d3ff");
                    }
                    else if (player.ZoneUnderworldHeight)
                    {
                        biomeColorResult = ColorHelper.HexToColor("ff8200");
                    }
                    else if (player.ZoneSnow)
                    {
                        biomeColorResult = ColorHelper.HexToColor("a0e9ff");
                    }

                    return biomeColorResult;
            }

            int wrapValue = 30;
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

        public void ApplyBiomeColors()
        {
            inkColorType = InkColorType.Biome;
            incrementType = IncrementType.Static;
            SetOverheadText("Now using biome colors!");
        }

        public void ApplyLifeStatColors()
        {
            inkColorType = InkColorType.LifePercentage;
            incrementType = IncrementType.StatBased;
            SetOverheadText("Now using colors based on life total!");
        }

        public void ApplyInkStatColors()
        {
            inkColorType = InkColorType.InkPercentage;
            incrementType = IncrementType.StatBased;
            SetOverheadText("Now using colors based on ink total!");
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
            tag[$"{nameof(initialColorSet)}"] = initialColorSet;

            tag[$"{nameof(colorA)}"] = colorA;
            tag[$"{nameof(colorB)}"] = colorB;

            tag[$"{nameof(inkColorType)}"] = inkColorType.ToString();
            tag[$"{nameof(incrementType)}"] = incrementType.ToString();
        }

        public override void LoadData(TagCompound tag)
        {
            initialColorSet = tag.GetBool($"{nameof(initialColorSet)}");

            if (initialColorSet)
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
        }

        #endregion
    }
}
