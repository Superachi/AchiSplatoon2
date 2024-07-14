using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode;
using AchiSplatoon2.Netcode.DataTransferObjects;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Color = Microsoft.Xna.Framework.Color;

namespace AchiSplatoon2.Content.Players
{
    internal class InkWeaponPlayer : BaseModPlayer
    {
        public int playerId = -1;
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
        public int SpecialIncrementCooldown = 0;
        public int SpecialIncrementCooldownDefault = 6;

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
        public float BlueChipBaseChargeBonus { get => 0.25f; }
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

        public float moveSpeedModifier = 1f;
        public float moveAccelModifier = 1f;
        public float moveFrictionModifier = 1f;
        public bool brushMoveSpeedCap = true;

        public override void PreUpdate()
        {
            if (SpecialIncrementCooldown > 0) SpecialIncrementCooldown--;

            if (playerId == -1 && DoesModPlayerBelongToLocalClient())
            {
                playerId = Player.whoAmI;
                Main.NewText($"Woomy! Hi {Player.name}!", Color.Orange);
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

                if (Main.rand.NextBool(10))
                {
                    dustId = Dust.NewDust(Position: pos,
                        Width: w,
                        Height: h,
                        Type: DustID.ShadowbeamStaff,
                        SpeedX: 0f,
                        SpeedY: 0f,
                        newColor: new Color(255, 255, 255),
                        Scale: Main.rand.NextFloat(1f, 2f));

                    dustInst = Main.dust[dustId];
                    dustInst.noLight = true;
                    dustInst.noLightEmittence = true;
                    dustInst.noGravity = true;
                    dustInst.fadeIn = 0f;
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
            }
            else
            {
                if (Player.HasBuff<SpecialReadyBuff>())
                {
                    Player.ClearBuff(ModContent.BuffType<SpecialReadyBuff>());
                }
            }

            AddSpecialPointsOnMovement();
            DrainSpecial();
        }

        public override void PostUpdateMiscEffects()
        {
            if (!DoesModPlayerBelongToLocalClient()) return;

            var oldMoveSpeedMod = moveSpeedModifier;
            var oldMoveAccelMod = moveAccelModifier;
            var oldMoveFrictionMod = moveFrictionModifier;
            moveSpeedModifier = 1f;
            moveAccelModifier = 1f;
            moveFrictionModifier = 1f;
            brushMoveSpeedCap = false;

            switch (Player.HeldItem.ModItem)
            {
                case Inkbrush:
                    if (Player.ItemTimeIsZero)
                    {
                        moveSpeedModifier = 1.1f;
                        moveAccelModifier = 5f;
                        moveFrictionModifier = 3f;
                    }
                    else
                    {
                        brushMoveSpeedCap = true;
                    }
                    break;
                case Octobrush:
                    if (Player.ItemTimeIsZero)
                    {
                        moveAccelModifier = 3f;
                    }
                    else
                    {
                        brushMoveSpeedCap = true;
                    }
                    break;
                case ClassicSquiffer:
                    if (Player.channel)
                    {
                        moveAccelModifier = 3f;
                        moveFrictionModifier = 3f;
                    }
                    break;
                case DarkTetraDualie:
                    moveAccelModifier = 3f;
                    if (Player.GetModPlayer<InkDualiePlayer>().postRollCooldown > 0)
                    {
                        moveFrictionModifier = 2f;
                    }
                    break;
            }

            // Move speed bonus from holding blue color chips
            if (IsPaletteValid() && !brushMoveSpeedCap)
            {
                int blueChipCount = ColorChipAmounts[(int)ChipColor.Blue];
                moveSpeedModifier       += blueChipCount * BlueChipBaseMoveSpeedBonus;
                moveAccelModifier       += blueChipCount * BlueChipBaseMoveSpeedBonus;
                moveFrictionModifier    += blueChipCount * BlueChipBaseMoveSpeedBonus;
            }

            if (oldMoveSpeedMod != moveSpeedModifier
            || oldMoveAccelMod != moveAccelModifier
            || oldMoveFrictionMod != moveFrictionModifier)
            {
                SyncMoveSpeedData();
            }
        }

        public override void PostUpdateRunSpeeds()
        {
            Player.maxRunSpeed *= moveSpeedModifier;
            Player.runAcceleration *= moveAccelModifier;
            Player.runSlowdown *= moveFrictionModifier;
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
            int chipCount = CalculateColorChipTotal();
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
            if (!DoesModPlayerBelongToLocalClient()) return;
            if (SpecialIncrementCooldown > 0) return;
            if (Player.dead) return;

            var accMP = Player.GetModPlayer<InkAccessoryPlayer>();

            if (!IsSpecialActive)
            {
                amount *= accMP.specialChargeMultiplier;
                SpecialPoints = Math.Clamp(SpecialPoints + amount, 0, SpecialPointsMax);
                SpecialIncrementCooldown += SpecialIncrementCooldownDefault;
            }

            if (SpecialPoints == SpecialPointsMax && !SpecialReady)
            {
                Player.AddBuff(ModContent.BuffType<SpecialReadyBuff>(), 2);
                CombatTextHelper.DisplayText("SPECIAL CHARGED!", Player.Center, color: new Color(255, 155, 0));
                SoundHelper.PlayAudio("Specials/SpecialReady", volume: 0.8f, pitchVariance: 0.1f, maxInstances: 1);
                SpecialReady = true;

                SyncModPlayerData();
            }
        }

        public void AddSpecialPointsForDamage(float amount)
        {
            // Increment at least 0.5%, but at most 10%
            float increment = Math.Clamp(amount, 0.5f, SpecialPointsMax / 10);
            IncrementSpecialPoints(increment);
        }

        private void AddSpecialPointsOnMovement()
        {
            if (Math.Abs(Player.velocity.X) > 1f)
            {
                float increment = 0.002f * Math.Abs(Player.velocity.X) * (ColorChipAmounts[(int)ChipColor.Blue] * BlueChipBaseChargeBonus);
                IncrementSpecialPoints(increment);
            }
        }

        public void ActivateSpecial(float drainSpeed, string specialName)
        {
            if (!DoesModPlayerBelongToLocalClient()) return;
            if (!IsSpecialActive)
            {
                SpecialName = specialName;
                IsSpecialActive = true;
                SpecialDrain = drainSpeed;
            }
        }

        public void DrainSpecial(float drainAmount = 0f)
        {
            if (!DoesModPlayerBelongToLocalClient()) return;
            if (Player.dead) return;

            if (IsSpecialActive)
            {
                if (drainAmount == 0f)
                {
                    SpecialPoints -= SpecialDrain;
                }
                else
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

        public void ResetSpecialStats()
        {
            if (!DoesModPlayerBelongToLocalClient()) return;

            IsSpecialActive = false;
            SpecialPoints = 0;
            SpecialDrain = 0;
            SpecialReady = false;
            SpecialName = null;

            // Worth noting that, due to how special drain is applied every update + every time the special is used,
            // This packet may get sent twice
            // Haven't been able to fix it yet (TODO)
            SyncModPlayerData();
        }

        public float CalculateSubDamageBonusModifier(bool hasMainWeaponBonus)
        {
            var accMP = Player.GetModPlayer<InkAccessoryPlayer>();

            float damageMod = 1f;
            damageMod *= accMP.subPowerMultiplier;
            if (hasMainWeaponBonus) damageMod *= (1 + BaseWeapon.subDamageBonus);
            return damageMod;
        }

        public float CalculateSpecialDamageBonusModifier()
        {
            var accMP = Player.GetModPlayer<InkAccessoryPlayer>();

            float damageMod = 1f;
            damageMod *= accMP.specialPowerMultiplier;
            return damageMod;
        }

        // NetCode
        private void SendPacket(PlayerPacketType msgType, object dto = null)
        {
            if (!DoesModPlayerBelongToLocalClient()) return;
            if (NetHelper.IsSinglePlayer()) return;

            string json = "";
            if (dto != null)
            {
                json = JsonConvert.SerializeObject(dto);
            }

            ModPlayerPacketHandler.SendModPlayerPacket(
                    msgType: msgType,
                    fromWho: Main.LocalPlayer.whoAmI,
                    json: json,
                    logger: Mod.Logger);
        }

        private void SyncModPlayerData()
        {
            var dto = new InkWeaponPlayerDTO(SpecialReady, ColorFromChips);
            SendPacket(PlayerPacketType.SyncModPlayer, dto);
        }

        private void SyncMoveSpeedData()
        {
            var dto = new PlayerMoveSpeedDTO(moveSpeedModifier, moveAccelModifier, moveFrictionModifier);
            SendPacket(PlayerPacketType.SyncMoveSpeed, dto);
        }

        public void UpdateInkColor(Color newColor)
        {
            if (ColorFromChips != newColor)
            {
                ColorFromChips = newColor;
                SyncModPlayerData();
            }
        }
    }
}
