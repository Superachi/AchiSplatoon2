using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Items.Accessories.InkTanks;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode;
using AchiSplatoon2.Netcode.DataTransferObjects;
using Microsoft.Xna.Framework;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class ColorChipPlayer : ModPlayer
    {
        public bool isPaletteEquipped;
        public int paletteCapacity;
        public bool conflictingPalettes;    // Is true if the player tries equipping more than one palette

        public int[] ColorChipAmounts = [0, 0, 0, 0, 0, 0];
        public int[] oldColorChipAmounts = [0, 0, 0, 0, 0, 0];
        public int ColorChipTotal;
        private Color _colorFromChips = ColorHelper.GetInkColor(InkColor.Order);
        public bool didColorChipAmountChange = false;

        public float RedChipBaseSubWeaponDamageBonus { get => 0.15f; }
        public string RedChipBaseSubWeaponDamageBonusDisplay { get => $"{(int)(RedChipBaseSubWeaponDamageBonus * 100)}%"; }
        public int RedChipBaseArmorPierceBonus { get => 2; }
        public string RedChipBaseArmorPierceBonusDisplay { get => $"{(RedChipBaseArmorPierceBonus)} Defense"; }
        public float PurpleSplatInkRecoveryBonus { get => 3f; }
        public string PurpleSplatInkRecoveryBonusDisplay { get => $"{PurpleSplatInkRecoveryBonus}%"; }
        public float PurpleChipBaseChargeSpeedBonus { get => 0.06f; }
        public string PurpleChipBaseChargeSpeedBonusDisplay { get => $"{(int)(PurpleChipBaseChargeSpeedBonus * 100)}%"; }
        public float YellowChipExplosionRadiusBonus { get => 0.08f; }
        public string YellowChipExplosionRadiusBonusDisplay { get => $"{(int)(YellowChipExplosionRadiusBonus * 100)}%"; }
        public float YellowChipVelocityBonus { get => 0.1f; }
        public string YellowChipVelocityBonusDisplay { get => $"{YellowChipVelocityBonus * 100}%"; }
        public float GreenChipLuckyBombChance { get => 0.15f; }
        public string GreenChipLuckyBombChanceDisplay { get => $"{(int)(GreenChipLuckyBombChance * 100)}%"; }
        public float GreenChipCritChanceBonus { get => 1f; }
        public string GreenChipCritChanceBonusDisplay { get => $"{(int)(GreenChipCritChanceBonus)}%"; }
        public float BlueChipBaseMoveSpeedBonus { get => 0.1f; }
        public string BlueChipBaseMoveSpeedBonusDisplay { get => $"{(int)(BlueChipBaseMoveSpeedBonus * 100)}%"; }
        public float BlueChipBaseChargeBonus { get => 1.5f; }
        public string BlueChipBaseChargeBonusDisplay { get => $"{BlueChipBaseChargeBonus.ToString("0.0")} point(s)"; }
        public float AquaChipBaseAttackCooldownReduction { get => 0.05f; }
        public string AquaChipBaseAttackCooldownReductionDisplay { get => $"{(int)(AquaChipBaseAttackCooldownReduction * 100)}%"; }

        public float PaletteMainDamageMod { get => 1.1f; }
        public string PaletteMainDamageModDisplay { get => $"+{(int)((PaletteMainDamageMod - 1) * 100)}%"; }

        public enum ChipColor
        {
            Red,
            Blue,
            Yellow,
            Purple,
            Green,
            Aqua,
        }

        public override void ResetEffects()
        {
            if (!NetHelper.IsPlayerSameAsLocalPlayer(Player)) return;

            didColorChipAmountChange = false;
            for (int i = 0; i < ColorChipAmounts.Length; i++)
            {
                if (ColorChipAmounts[i] != oldColorChipAmounts[i])
                {
                    didColorChipAmountChange = true;
                    break;
                }
            }

            for (int i = 0; i < ColorChipAmounts.Length; i++)
            {
                oldColorChipAmounts[i] = ColorChipAmounts[i];
            }

            conflictingPalettes = false;
            isPaletteEquipped = false;
            paletteCapacity = 0;
            ColorChipAmounts = [0, 0, 0, 0, 0, 0];
            ColorChipTotal = 0;
        }

        public override void PostUpdate()
        {
            if (didColorChipAmountChange)
            {
                SetDefaultInkColorBasedOnColorChips();
                SyncColorChipData();
            }
        }


        public bool DoesPlayerHaveTooManyChips()
        {
            int chipCount = CalculateColorChipTotal();
            return chipCount > paletteCapacity;
        }

        public bool DoesPlayerHaveEqualAmountOfChips()
        {
            int lastAmount = 0;
            for (int i = 0; i < ColorChipAmounts.Length; i++)
            {
                if (i > 0 && ColorChipAmounts[i] != lastAmount)
                {
                    return false;
                }

                lastAmount = ColorChipAmounts[i];
            }
            return true;
        }

        public bool IsPaletteValid()
        {
            return !conflictingPalettes && !DoesPlayerHaveTooManyChips();
        }

        public int CalculateColorChipTotal()
        {
            var total = 0;
            for (int i = 0; i < ColorChipAmounts.Length; i++)
            {
                total += ColorChipAmounts[i];
            }
            return total;
        }

        public float CalculateAttackDamageBonus()
        {
            return ColorChipAmounts[(int)ChipColor.Red] * RedChipBaseSubWeaponDamageBonus;
        }

        public int CalculateArmorPierceBonus()
        {
            return ColorChipAmounts[(int)ChipColor.Red] * RedChipBaseArmorPierceBonus;
        }

        public float CalculateChargeSpeedBonus()
        {
            return ColorChipAmounts[(int)ChipColor.Purple] * PurpleChipBaseChargeSpeedBonus;
        }

        public float CalculateSplatInkRecoveryBonus()
        {
            return ColorChipAmounts[(int)ChipColor.Purple] * PurpleSplatInkRecoveryBonus;
        }

        public float CalculateExplosionRadiusBonus()
        {
            return ColorChipAmounts[(int)ChipColor.Yellow] * YellowChipExplosionRadiusBonus;
        }

        public float CalculateProjectileVelocityBonus()
        {
            return ColorChipAmounts[(int)ChipColor.Yellow] * YellowChipVelocityBonus;
        }

        public float CalculateCritChanceBonus()
        {
            return ColorChipAmounts[(int)ChipColor.Green] * GreenChipCritChanceBonus;
        }

        public float CalculateLuckyBombChance()
        {
            return ColorChipAmounts[(int)ChipColor.Green] * GreenChipLuckyBombChance;
        }

        public float CalculateDroneAttackCooldownReduction()
        {
            return ColorChipAmounts[(int)ChipColor.Aqua] * AquaChipBaseAttackCooldownReduction;
        }

        public float CalculateMobilitySpecialChargeIncrement()
        {
            return ColorChipAmounts[(int)ChipColor.Blue] * BlueChipBaseChargeBonus;
        }

        public void UpdateInkColor()
        {
            SetDefaultInkColorBasedOnColorChips();
        }

        public Color GetColorFromChips()
        {
            if (Player.HasAccessory<EmpressInkTank>()
                || (DoesPlayerHaveEqualAmountOfChips() && CalculateColorChipTotal() != 0))
            {
                return ColorHelper.LerpBetweenColorsPerfect(Main.DiscoColor, Color.White, 0.2f);
            }

            if (Player.HeldItem.ModItem != null && Player.HeldItem.ModItem.GetType().GetCustomAttribute<ShimmerOrderWeaponAttribute>() != null)
            {
                return ColorHelper.LerpBetweenColorsPerfect(Main.DiscoColor, Color.White, 0.7f);
            }

            return IsPaletteValid() ? _colorFromChips : ColorHelper.GetInkColor(InkColor.Order);
        }

        private void SetDefaultInkColorBasedOnColorChips()
        {
            var primaryHighest = 0;
            var secondaryHighest = 0;
            var primaryColor = InkColor.Order;
            var secondaryColor = InkColor.Order;

            for (int i = 0; i < ColorChipAmounts.Length; i++)
            {
                int value = ColorChipAmounts[i];

                // Only consider the color if we have any chips for it
                if (value > 0)
                {
                    // Change the primary color if we see a new highest count
                    if (value > primaryHighest)
                    {
                        // If we've no other colors, make the secondary color match the primary one
                        if (secondaryHighest == 0)
                        {
                            secondaryColor = (InkColor)i;
                            secondaryHighest = value;
                        }
                        // If we do, mark the previous primary color as the secondary color
                        else
                        {
                            secondaryColor = primaryColor;
                            secondaryHighest = primaryHighest;
                        }

                        primaryColor = (InkColor)i;
                        primaryHighest = value;
                    }
                    // What if we don't have the highest count?
                    else if (primaryColor == secondaryColor || value > secondaryHighest)
                    {
                        secondaryColor = (InkColor)i;
                        secondaryHighest = value;
                    }
                }
            }

            // If there are two color chips being considered, add a bias towards the color that we have more chips of
            _colorFromChips = ColorHelper.CombinePrimarySecondaryColors(
                ColorHelper.GetInkColor(primaryColor),
                ColorHelper.GetInkColor(secondaryColor));

            if (primaryHighest != secondaryHighest)
            {
                _colorFromChips = ColorHelper.CombinePrimarySecondaryColors(
                ColorHelper.GetInkColor(primaryColor),
                ColorHelper.GetInkColor(secondaryColor),
                ColorHelper.GetInkColor(primaryColor));
            };

            if (primaryHighest == 0 && secondaryHighest == 0)
            {
                _colorFromChips = ColorHelper.CombinePrimarySecondaryColors(ColorHelper.GetInkColor(primaryColor), ColorHelper.GetInkColor(secondaryColor));
            }
        }

        #region Netcode

        public void SyncAllDataManual()
        {
            if (NetHelper.IsSinglePlayer()) return;

            SyncColorChipData();
        }

        private void SyncColorChipData()
        {
            var dto = new ColorChipPlayerDTO(colorChipAmounts: ColorChipAmounts);

            NetHelper.SendModPlayerPacket(this, PlayerPacketType.ColorChipPlayer, dto);
        }

        #endregion
    }
}
