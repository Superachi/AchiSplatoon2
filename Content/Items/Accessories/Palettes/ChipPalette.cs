using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static AchiSplatoon2.Content.Players.ColorChipPlayer;

namespace AchiSplatoon2.Content.Items.Accessories.Palettes
{
    internal class ChipPalette : BaseAccessory
    {
        protected virtual int PaletteCapacity { get => 4; }
        protected virtual MainWeaponStyle WeaponStyle { get => MainWeaponStyle.Other; }
        public MainWeaponStyle PaletteWeaponStyle() => WeaponStyle;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 20;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            AddRecipeWithSheldonLicenseBasic(registerNow: false)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!NetHelper.IsPlayerSameAsLocalPlayer(player)) return;

            var modPlayer = player.GetModPlayer<ColorChipPlayer>();

            if (modPlayer.isPaletteEquipped)
            {
                modPlayer.conflictingPalettes = true;
                return;
            }

            modPlayer.isPaletteEquipped = true;
            modPlayer.paletteCapacity = PaletteCapacity;

            // Note that disabling the buffs here doesn't disable ALL the buffs
            // See also the calculations in BaseProjectile.cs
            if (!modPlayer.IsPaletteValid()) return;

            var accMP = player.GetModPlayer<AccessoryPlayer>();
            accMP.paletteType = GetType();
        }

        public override bool CanReforge()
        {
            return false;
        }

        public override bool AllowPrefix(int pre)
        {
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;

            var textColorEffect = "c/ff8e2c:";
            var textColorWhite = "c/ffffff:";
            var textColorGray = "c/a8a8a8:";
            var textColorWarn = "c/ed3a4a:";
            string ChipTextColor(InkColor color) => ColorHelper.GetChipTextColor(color);

            var modPlayer = player.GetModPlayer<ColorChipPlayer>();
            int index = tooltips.FindIndex(l => l.Name == "ItemName");
            if (index != -1)
            {
                var t = tooltips[index];
                if (modPlayer.isPaletteEquipped)
                {
                    // Display an error tooltip if requirements aren't met
                    if (modPlayer.conflictingPalettes)
                    {
                        t.Text = $"{Item.Name}" +
                            $"\n[{textColorWarn}ERROR! Palette inactive!]" +
                            $"\n[{textColorGray}You cannot equip multiple palettes at the same time.]";
                        return;
                    }

                    int chipCount = modPlayer.CalculateColorChipTotal();
                    if (modPlayer.DoesPlayerHaveTooManyChips())
                    {
                        t.Text = $"{Item.Name}" +
                            $"\n[{textColorWarn}ERROR! Palette inactive!]" +
                            $"\n[{textColorGray}Remove Color Chips from your inventory until you have {PaletteCapacity} or less.]" +
                            $"\n[{textColorGray}Currently you have {chipCount} Color Chips in your inventory.]";
                        return;
                    }

                    // Palette name, main weapon boost type and chip count
                    t.Text = $"{Item.Name}";
                    if (WeaponStyle != MainWeaponStyle.Other)
                    {
                        string boostedWeapon = PaletteWeaponStyle().ToString();
                        t.Text += "\n" + ColorHelper.TextWithBonusColor($"{modPlayer.PaletteMainDamageModDisplay} {boostedWeapon} damage");
                    }

                    t.Text += "\n" + ColorHelper.TextWithFunctionalColor("Activates the effects of Color Chips") +
                        "\n" + ColorHelper.TextWithFunctionalColor("held in your inventory ") +
                        ColorHelper.TextWithNeutralColor($"({chipCount}/{PaletteCapacity})");

                    // List color chip bonuses
                    t.Text += $"\n[{textColorWhite}Currently active bonuses:]";

                    var chips = modPlayer.ColorChipAmounts;
                    var red = (float)chips[(int)ChipColor.Red];
                    var blue = (float)chips[(int)ChipColor.Blue];
                    var yellow = (float)chips[(int)ChipColor.Yellow];
                    var purple = (float)chips[(int)ChipColor.Purple];
                    var green = (float)chips[(int)ChipColor.Green];
                    var aqua = (float)chips[(int)ChipColor.Aqua];

                    if (red > 0)
                    {
                        t.Text += $"\n[{ChipTextColor(InkColor.Red)}Power ({red}) >]"
                            + "\n" + ColorHelper.TextWithFlavorColor("Damage: ")
                            + ColorHelper.TextWithBonusColor($"+{(int)(red * modPlayer.RedChipBaseAttackDamageBonus * 100)}%")
                            + "\n" + ColorHelper.TextWithFlavorColor("Armor penetration: ")
                            + ColorHelper.TextWithBonusColor($"{(int)(red * modPlayer.RedChipBaseArmorPierceBonus)} Defense");
                    }
                    if (blue > 0)
                    {
                        t.Text += $"\n[{ChipTextColor(InkColor.Blue)}Mobility ({blue}) >]"
                            + "\n" + ColorHelper.TextWithFlavorColor("Movement speed: ")
                            + ColorHelper.TextWithBonusColor($"+{(int)(blue * modPlayer.BlueChipBaseMoveSpeedBonus * 100)}%")
                            + "\n" + ColorHelper.TextWithFlavorColor("Movement special charge with roller & brush: ")
                            + ColorHelper.TextWithBonusColor($"{(blue * modPlayer.BlueChipBaseChargeBonus).ToString("0.0")} point(s)");
                    }
                    if (yellow > 0)
                    {
                        t.Text += $"\n[{ChipTextColor(InkColor.Yellow)}Range ({yellow}) >]"
                            + "\n" + ColorHelper.TextWithFlavorColor("Explosion radius: ")
                            + ColorHelper.TextWithBonusColor($"+{(int)(yellow * modPlayer.YellowChipExplosionRadiusBonus * 100)}%")
                            + "\n" + ColorHelper.TextWithFlavorColor("Shot velocity & accuracy: ")
                            + ColorHelper.TextWithBonusColor($"+{(int)(yellow * modPlayer.YellowChipVelocityBonus * 100)}%");
                    }
                    if (purple > 0)
                    {
                        t.Text += $"\n[{ChipTextColor(InkColor.Purple)}Support ({purple}) >]"
                            + "\n" + ColorHelper.TextWithFlavorColor("Ink recovery on special charge gain: ")
                            + ColorHelper.TextWithBonusColor($"+{(int)(purple * modPlayer.PurpleSplatInkRecoveryBonus)}%")
                            + "\n" + ColorHelper.TextWithFlavorColor("Weapon charge speed: ")
                            + ColorHelper.TextWithBonusColor($"+{(int)(purple * modPlayer.PurpleChipBaseChargeSpeedBonus * 100)}%");
                    }
                    if (green > 0)
                    {
                        t.Text += $"\n[{ChipTextColor(InkColor.Green)}Lucky ({green}) >]"
                            + "\n" + ColorHelper.TextWithFlavorColor("Lucky bomb drop chance: ")
                            + ColorHelper.TextWithBonusColor($"+{(int)(modPlayer.CalculateLuckyBombChance() * 100)}%");
                    }
                    if (aqua > 0)
                    {
                        t.Text += $"\n[{ChipTextColor(InkColor.Aqua)}Drone ({aqua}) >]"
                            + "\n" + ColorHelper.TextWithFlavorColor("Pearl Drone attack speed: ")
                            + ColorHelper.TextWithBonusColor($"+{(int)(modPlayer.CalculateDroneAttackCooldownReduction() * 100)}%");
                    }
                }
                else
                {
                    t.Text = $"{Item.Name}" +
                        $"\n[{textColorEffect}When equipped, it activates the effects of Color Chips held in your inventory (up to {PaletteCapacity})]";
                }
            }
        }
    }
}
