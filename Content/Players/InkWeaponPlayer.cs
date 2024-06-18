using AchiSplatoon2.Helpers;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class InkWeaponPlayer : ModPlayer
    {
        public bool isPaletteEquipped;
        public int paletteCapacity;
        public bool conflictingPalettes;    // Is true if the player tries equipping more than one palette

        public int[] ColorChipAmounts;
        public int ColorChipTotal;

        public float SpecialPoints;
        public float SpecialPointsMax = 100;
        public bool SpecialReady;
        public bool IsSpecialActive;
        public float SpecialDrain;

        public float RedChipBaseAttackSpeedBonus { get => 0.04f; }
        public string RedChipBaseAttackSpeedBonusDisplay { get => $"{(int)(RedChipBaseAttackSpeedBonus * 100)}%"; }
        public float PurpleChipBaseKnockbackBonus { get => 0.5f; }
        public string PurpleChipBaseKnockbackBonusDisplay { get => $"{PurpleChipBaseKnockbackBonus} unit(s)"; }
        public float PurpleChipBaseChargeSpeedBonus { get => 0.06f; }
        public string PurpleChipBaseChargeSpeedBonusDisplay { get => $"{(int)(PurpleChipBaseChargeSpeedBonus * 100)}%"; }
        public float YellowChipExplosionRadiusBonus { get => 0.1f; }
        public string YellowChipExplosionRadiusBonusDisplay { get => $"{(int)(YellowChipExplosionRadiusBonus * 100)}%"; }
        public int YellowChipPiercingBonus { get => 1; }
        public string YellowChipPiercingBonusDisplay { get => $"{YellowChipPiercingBonus}"; }
        public float GreenChipBaseCritBonus { get => 4f; }
        public string GreenChipBaseCritBonusDisplay { get => $"{GreenChipBaseCritBonus}%"; }
        public float BlueChipBaseMoveSpeedBonus { get => 0.2f; }
        public string BlueChipBaseMoveSpeedBonusDisplay { get => $"{(int)(BlueChipBaseMoveSpeedBonus * 100)}%"; }

        public enum ChipColor
        {
            Red,
            Blue,
            Yellow,
            Purple,
            Green,
            Aqua,
        }

        public override void PreUpdate()
        {
            DrainSpecial();
        }

        public override void ResetEffects()
        {
            conflictingPalettes = false;
            isPaletteEquipped = false;
            paletteCapacity = 0;
            ColorChipAmounts = [0, 0, 0, 0, 0, 0];
            ColorChipTotal = 0;
        }

        public bool DoesPlayerHaveTooManyChips()
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();
            int chipCount = modPlayer.CalculateColorChipTotal();
            return (chipCount > paletteCapacity);
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

        public float CalculateAttackSpeedBonus()
        {
            return ColorChipAmounts[(int)ChipColor.Red] * RedChipBaseAttackSpeedBonus;
        }

        public float CalculateChargeSpeedBonus()
        {
            return ColorChipAmounts[(int)ChipColor.Purple] * PurpleChipBaseChargeSpeedBonus;
        }

        public float CalculateExplosionRadiusBonus()
        {
            return ColorChipAmounts[(int)ChipColor.Yellow] * YellowChipExplosionRadiusBonus;
        }

        public int CalculatePiercingBonus()
        {
            return ColorChipAmounts[(int)ChipColor.Yellow];
        }

        public void AddSpecialPoints(int damage)
        {
            // Clamp the amount of points gained between 1 and the max divided by 10 (meaning at least 10 hits)
            float increment = Math.Clamp(damage / 10, 0.2f, SpecialPointsMax / 10);
            SpecialPoints += increment;

            if (SpecialPoints > SpecialPointsMax)
            {
                SpecialPoints = SpecialPointsMax;
                if (!SpecialReady) { SpecialReady = true; }
            }
        }

        public void ActivateSpecial(float drainSpeed)
        {
            if (!IsSpecialActive)
            {
                CombatTextHelper.DisplayText("Activating special!", Main.LocalPlayer.Center);
                IsSpecialActive = true;
                SpecialDrain = drainSpeed;
            }
        }

        public void DrainSpecial(float drainAmount = 0f)
        {
            if (IsSpecialActive)
            {
                if (drainAmount == 0f)
                {
                    SpecialPoints -= SpecialDrain;
                } else
                {
                    SpecialPoints -= drainAmount;
                }

                if (SpecialPoints <= 0)
                {
                    CombatTextHelper.DisplayText("Special is finished", Main.LocalPlayer.Center);
                    IsSpecialActive = false;
                    SpecialPoints = 0;
                    SpecialDrain = 0;
                    SpecialReady = false;
                }
            }
        }
    }
}
