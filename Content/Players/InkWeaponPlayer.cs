using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class InkWeaponPlayer : ModPlayer
    {
        public bool isPaletteEquipped;
        public int paletteCapacity;
        public bool conflictingPalettes;    // Is true if the player tries equipping more than one palette

        // Color chip
        public int[] ColorChipAmounts;
        public int ColorChipTotal;
        public Color ColorFromChips = ColorHelper.GetInkColor(InkColor.Order);

        // Special gauge
        public float SpecialPoints;
        public float SpecialPointsMax = 100;
        public bool SpecialReady;
        public bool IsSpecialActive;
        public string SpecialName = null;
        public float SpecialDrain;

        // Accessories
        public bool hasSpecialPowerEmblem;
        public bool hasSpecialChargeEmblem;
        public bool hasSubPowerEmblem;
        public static float specialChargeMultiplier = 1.5f;
        public static float subPowerMultiplier = 2f;
        public static float specialPowerMultiplier = 2f;

        public float RedChipBaseAttackDamageBonus { get => 0.03f; }
        public string RedChipBaseAttackDamageBonusDisplay { get => $"{(int)(RedChipBaseAttackDamageBonus * 100)}%"; }
        public int RedChipBaseArmorPierceBonus { get => 2; }
        public string RedChipBaseArmorPierceBonusDisplay { get => $"{(RedChipBaseArmorPierceBonus)} Defense"; }
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
        public float GreenChipLootBonusDivider { get => 2f; }
        public float BlueChipBaseMoveSpeedBonus { get => 0.15f; }
        public string BlueChipBaseMoveSpeedBonusDisplay { get => $"{(int)(BlueChipBaseMoveSpeedBonus * 100)}%"; }
        public float BlueChipBaseChargeBonus { get => 0.5f; }
        public string BlueChipBaseChargeBonusDisplay { get => $"{(int)(BlueChipBaseChargeBonus * 100)}%"; }

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
            Player player = Main.LocalPlayer;
            if (player.HeldItem.ModItem is Inkbrush && player.ItemTimeIsZero)
            {
                player.maxRunSpeed *= 1.1f;
                player.runAcceleration *= 5f;
            } else if (player.HeldItem.ModItem is Octobrush && player.ItemTimeIsZero)
            {
                player.runAcceleration *= 3f;
            }

            // Emit dusts when special is ready
            if (SpecialReady)
            {
                var w = 40;
                var h = 60;
                var pos = Player.position - new Vector2(w / 2, 0);
                int dustId;
                Dust dustInst;

                if (Main.rand.NextBool(2))
                {
                    dustId = Dust.NewDust(Position: pos,
                        Width: w,
                        Height: h,
                        Type: DustID.AncientLight,
                        SpeedX: 0f,
                        SpeedY: -2.5f,
                        newColor: ColorFromChips,
                        Scale: Main.rand.NextFloat(1f, 2f));

                    dustInst = Main.dust[dustId];
                    dustInst.noGravity = true;
                    dustInst.fadeIn = 1.05f;
                }

                if (Main.rand.NextBool(4))
                {
                    h = 20;
                    pos = Player.position - new Vector2(w / 2, h);
                    dustId = Dust.NewDust(Position: pos,
                    Width: w,
                    Height: h,
                    Type: ModContent.DustType<SplatterBulletDust>(),
                    SpeedX: Main.rand.NextFloat(-2f, 2f),
                    SpeedY: -5f,
                    Alpha: 40,
                    newColor: ColorFromChips,
                    Scale: 2f);

                    dustInst = Main.dust[dustId];
                    dustInst.noGravity = true;
                    dustInst.fadeIn = 1.35f;
                }
            } else
            {
                if (player.HasBuff<SpecialReadyBuff>())
                {
                    player.ClearBuff(ModContent.BuffType<SpecialReadyBuff>());
                }
            }

            AddSpecialPointsOnMovement();
            DrainSpecial();
        }

        public override void ResetEffects()
        {
            conflictingPalettes = false;
            isPaletteEquipped = false;
            paletteCapacity = 0;
            ColorChipAmounts = [0, 0, 0, 0, 0, 0];
            ColorChipTotal = 0;
            hasSpecialPowerEmblem = false;
            hasSpecialChargeEmblem = false;
            hasSubPowerEmblem = false;
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

        public float CalculateAttackDamageBonus()
        {
            return ColorChipAmounts[(int)ChipColor.Red] * RedChipBaseAttackDamageBonus;
        }

        public int CalculateArmorPierceBonus()
        {
            return ColorChipAmounts[(int)ChipColor.Red] * RedChipBaseArmorPierceBonus;
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

        public void IncrementSpecialPoints(float amount)
        {
            var player = Main.LocalPlayer;
            if (player.dead) return;

            if (!IsSpecialActive)
            {
                if (hasSpecialChargeEmblem) { amount *= specialChargeMultiplier; }
                SpecialPoints = Math.Clamp(SpecialPoints + amount, 0, SpecialPointsMax);
            }

            if (SpecialPoints == SpecialPointsMax && !SpecialReady)
            {
                player.AddBuff(ModContent.BuffType<SpecialReadyBuff>(), 2);
                CombatTextHelper.DisplayText("SPECIAL CHARGED!", player.Center, color: new Color(255, 155, 0));
                SoundHelper.PlayAudio("Specials/SpecialReady", volume: 0.8f, pitchVariance: 0.1f, maxInstances: 1);
                SpecialReady = true;
            }
        }

        public void AddSpecialPointsForDamage(float amount)
        {
            // Increment at least 0.5%, but at most 10%
            float increment = Math.Clamp(amount, 0.5f, SpecialPointsMax / 10);
            IncrementSpecialPoints(increment);
        }

        private void AddSpecialPointsOnKill()
        {
            IncrementSpecialPoints(SpecialPointsMax/5);
        }

        private void AddSpecialPointsOnMovement()
        {
            var player = Main.LocalPlayer;
            if (Math.Abs(player.velocity.X) > 1f) {
                float increment = 0.002f * Math.Abs(player.velocity.X) * (1 + ColorChipAmounts[(int)ChipColor.Blue] * BlueChipBaseChargeBonus);
                IncrementSpecialPoints(increment);
            }
        }

        public void ActivateSpecial(float drainSpeed, string specialName)
        {
            if (!IsSpecialActive)
            {
                SpecialName = specialName;
                IsSpecialActive = true;
                SpecialDrain = drainSpeed;
            }
        }

        public void DrainSpecial(float drainAmount = 0f)
        {
            var player = Main.LocalPlayer;
            if (player.dead) return;

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
                    ResetSpecialStats();
                }
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            ResetSpecialStats();
        }

        public void ResetSpecialStats() {
            IsSpecialActive = false;
            SpecialPoints = 0;
            SpecialDrain = 0;
            SpecialReady = false;
            SpecialName = null;
        }
    }
}
