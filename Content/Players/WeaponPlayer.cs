using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Rollers;
using AchiSplatoon2.Content.Prefixes.BrushPrefixes;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Helpers.WeaponKits;
using AchiSplatoon2.Netcode;
using AchiSplatoon2.Netcode.DataTransferObjects;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static AchiSplatoon2.Content.Players.ColorChipPlayer;
using Color = Microsoft.Xna.Framework.Color;

namespace AchiSplatoon2.Content.Players
{
    internal class WeaponPlayer : ModPlayer
    {
        // Special gauge
        public float SpecialPoints;
        public float SpecialPointsMax = 100;
        public bool SpecialReady;
        public bool IsSpecialActive;
        public string? SpecialName = null;
        public float SpecialDrain;
        public int SpecialIncrementCooldown = 0;
        public int SpecialIncrementCooldownDefault = 6;

        public int CustomWeaponCooldown = 0;

        public float moveSpeedModifier = 1f;
        public float moveAccelModifier = 1f;
        public float moveFrictionModifier = 1f;

        public bool isUsingRoller = false;
        public bool isBrushRolling = false;
        public bool isBrushAttacking = false;

        private ColorChipPlayer colorChipPlayer => Player.GetModPlayer<ColorChipPlayer>();

        public override void PreUpdate()
        {
            if (CustomWeaponCooldown > 0) CustomWeaponCooldown--;
            if (SpecialIncrementCooldown > 0) SpecialIncrementCooldown--;

            if (SpecialReady && !Player.HasBuff<SpecialReadyBuff>())
            {
                ResetSpecialStats();
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
                        newColor: colorChipPlayer.GetColorFromChips(),
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
                    newColor: colorChipPlayer.GetColorFromChips(),
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

        // UpdateInventory

        public override void PostUpdateMiscEffects()
        {
            if (!NetHelper.IsPlayerSameAsLocalPlayer(Player)) return;

            var oldMoveSpeedMod = moveSpeedModifier;
            var oldMoveAccelMod = moveAccelModifier;
            var oldMoveFrictionMod = moveFrictionModifier;
            moveSpeedModifier = 1f;
            moveAccelModifier = 1f;
            moveFrictionModifier = 1f;

            float mountSpeedMultMod = 1f;
            if (Player.mount.Active)
            {
                mountSpeedMultMod = 0.5f;
            }

            switch (Player.HeldItem.ModItem)
            {
                case BaseBrush:
                    BaseBrush brush = (BaseBrush)Player.HeldItem.ModItem;

                    if (isBrushRolling)
                    {
                        moveSpeedModifier = brush.RollMoveSpeedBonus;
                        moveAccelModifier = 2f * brush.RollMoveSpeedBonus;
                        moveFrictionModifier = 2f * brush.RollMoveSpeedBonus;

                        var prefix = PrefixHelper.GetWeaponPrefixById(Player.HeldItem.prefix);
                        if (prefix is BaseBrushPrefix brushPrefix)
                        {
                            moveSpeedModifier *= brushPrefix.DashSpeedModifier.NormalizePrefixMod();
                            moveAccelModifier *= brushPrefix.DashSpeedModifier.NormalizePrefixMod();
                            moveFrictionModifier *= brushPrefix.DashSpeedModifier.NormalizePrefixMod();
                        }
                    }
                    break;
                case BaseRoller:
                    var roller = (BaseRoller)Player.HeldItem.ModItem;
                    if (isUsingRoller)
                    {
                        moveAccelModifier = Math.Max(1, roller.RollingAccelModifier);
                        moveFrictionModifier = Math.Max(1, roller.RollingAccelModifier);
                    }
                    break;
            }

            if (Player.HeldItem.ModItem is BaseDualie)
            {
                moveAccelModifier = 2f;
                if (Player.GetModPlayer<DualiePlayer>().postRollCooldown > 0)
                {
                    moveFrictionModifier = 2f;
                }
            }

            // Move speed bonus from holding blue color chips
            if (colorChipPlayer.IsPaletteValid())
            {
                int blueChipCount = colorChipPlayer.ColorChipAmounts[(int)ChipColor.Blue];
                moveSpeedModifier += blueChipCount * colorChipPlayer.BlueChipBaseMoveSpeedBonus * mountSpeedMultMod;
                moveAccelModifier += blueChipCount * colorChipPlayer.BlueChipBaseMoveSpeedBonus * mountSpeedMultMod;
                moveFrictionModifier += blueChipCount * colorChipPlayer.BlueChipBaseMoveSpeedBonus * mountSpeedMultMod;
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


        public void IncrementSpecialPoints(float amount)
        {
            if (!NetHelper.IsPlayerSameAsLocalPlayer(Player)) return;
            if (SpecialIncrementCooldown > 0) return;
            if (Player.dead) return;

            var accMP = Player.GetModPlayer<AccessoryPlayer>();

            if (!IsSpecialActive)
            {
                amount *= accMP.specialChargeMultiplier;
                SpecialPoints = Math.Clamp(SpecialPoints + amount, 0, SpecialPointsMax);
            }

            if (SpecialPoints == SpecialPointsMax && !SpecialReady)
            {
                Player.AddBuff(ModContent.BuffType<SpecialReadyBuff>(), 60 * 30);
                CombatTextHelper.DisplayText("SPECIAL CHARGED!", Player.Center, color: new Color(255, 155, 0));
                SoundHelper.PlayAudio("Specials/SpecialReady", volume: 0.8f, pitchVariance: 0.1f, maxInstances: 1);
                SpecialReady = true;

                SyncSpecialChargeData();
            }
        }

        public void AddSpecialPointsForDamage(float amount)
        {
            IncrementSpecialPoints(amount);
            SpecialIncrementCooldown += SpecialIncrementCooldownDefault;
        }

        private void AddSpecialPointsOnMovement()
        {
            if (Math.Abs(Player.velocity.X) > 1f)
            {
                float increment = 0.002f * Math.Abs(Player.velocity.X) * (colorChipPlayer.ColorChipAmounts[(int)ChipColor.Blue] * colorChipPlayer.BlueChipBaseChargeBonus);
                IncrementSpecialPoints(increment);
            }
        }

        public void ActivateSpecial(float drainSpeed, Item special)
        {
            if (!NetHelper.IsPlayerSameAsLocalPlayer(Player)) return;
            if (!IsSpecialActive)
            {
                if (SpecialPoints == SpecialPointsMax)
                {
                    var dronePlayer = Player.GetModPlayer<PearlDronePlayer>();
                    dronePlayer.TriggerDialoguePlayerActivatesSpecial(special.type);
                }

                SpecialName = special.Name;
                IsSpecialActive = true;
                SpecialDrain = drainSpeed;
            }
        }

        public void DrainSpecial(float drainAmount = 0f)
        {
            if (!NetHelper.IsPlayerSameAsLocalPlayer(Player)) return;
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
            if (!NetHelper.IsPlayerSameAsLocalPlayer(Player)) return;

            IsSpecialActive = false;
            SpecialPoints = 0;
            SpecialDrain = 0;
            SpecialReady = false;
            SpecialName = null;

            SyncSpecialChargeData();
        }

        public float CalculateSubDamageBonusModifier(bool hasMainWeaponBonus)
        {
            var accMP = Player.GetModPlayer<AccessoryPlayer>();
            var heldItem = Player.HeldItem.ModItem;

            float damageMod = 1f;
            if (heldItem is BaseWeapon)
            {
                damageMod *= accMP.subPowerMultiplier;
                if (hasMainWeaponBonus) damageMod *= (1 + WeaponKitList.GetWeaponKitSubBonusAmount(heldItem.GetType()));
            }
            return damageMod;
        }

        public float CalculateSpecialDamageBonusModifier()
        {
            var accMP = Player.GetModPlayer<AccessoryPlayer>();

            float damageMod = 1f;
            damageMod *= accMP.specialPowerMultiplier;
            return damageMod;
        }

        // NetCode
        public override void OnEnterWorld()
        {
            SyncAllDataIfMultiplayer();
        }

        private void SendPacket(WeaponPlayerDTO dto)
        {
            if (!NetHelper.IsPlayerSameAsLocalPlayer(Player)) return;
            if (NetHelper.IsSinglePlayer()) return;

            NetHelper.SendModPlayerPacket(this, PlayerPacketType.WeaponPlayer, dto);
        }

        public void SyncAllDataManual()
        {
            SyncAllDataIfMultiplayer();
        }

        private void SyncAllDataIfMultiplayer()
        {
            if (NetHelper.IsSinglePlayer()) return;

            SyncSpecialChargeData();
            SyncMoveSpeedData();
        }

        private void SyncSpecialChargeData()
        {
            var dto = new WeaponPlayerDTO(
                specialReady: SpecialReady);

            SendPacket(dto);
        }

        private void SyncMoveSpeedData()
        {
            var dto = new WeaponPlayerDTO(
                moveSpeedMod: moveSpeedModifier,
                moveAccelMod: moveAccelModifier,
                moveFrictionMod: moveFrictionModifier);

            SendPacket(dto);
        }
    }
}
