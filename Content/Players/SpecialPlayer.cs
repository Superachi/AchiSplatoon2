using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.SpecialProjectiles;
using AchiSplatoon2.Content.Items.Weapons.Specials;

namespace AchiSplatoon2.Content.Players
{
    internal class SpecialPlayer : ModPlayer
    {
        public float SpecialPoints;
        public float SpecialPointsMax = 100;
        public bool SpecialReady;
        public bool SpecialActivated;
        public float SpecialDrainRate;

        private ColorChipPlayer? _colorChipPlayer;
        private HudPlayer? _hudPlayer;

        public float SpecialPercentage => MathHelper.Clamp(SpecialPoints / SpecialPointsMax, 0, 1);
        private float _specialDisplayPercentage;

        public override void Initialize()
        {
            _specialDisplayPercentage = 0f;
            _colorChipPlayer = Player.GetModPlayer<ColorChipPlayer>();
            _hudPlayer = Player.GetModPlayer<HudPlayer>();
        }

        public override void PreUpdate()
        {
            _specialDisplayPercentage = MathHelper.Lerp(_specialDisplayPercentage, SpecialPercentage, 0.2f);

            if (SpecialPercentage >= 1 && !SpecialReady)
            {
                ReadySpecial();
            }

            if (SpecialReady ^ Player.HasBuff<SpecialReadyBuff>())
            {
                UnreadySpecial();
                return;
            }

            bool middleClicked = InputHelper.GetInputMiddleClicked();
            if (!SpecialReady)
            {
                if (middleClicked && !_hudPlayer!.IsTextActive())
                {
                    _hudPlayer!.SetOverheadText("Your special isn't ready yet!", 60);
                    SoundHelper.PlayAudio(SoundPaths.EmptyInkTank.ToSoundStyle(), volume: 0.5f);
                }
                return;
            }

            if (middleClicked)
            {
                var success = TryActivateSpecial();
                if (success)
                {
                    SpecialActivated = true;

                    var inkTankPlayer = Player.GetModPlayer<InkTankPlayer>();
                    inkTankPlayer.HealInk(inkTankPlayer.InkAmountFinalMax, true);
                }
            }

            if (SpecialActivated)
            {
                DrainSpecialCharge();
            }

            SpecialDustStream();
        }

        private void ReadySpecial()
        {
            SpecialReady = true;
            Player.AddBuff(ModContent.BuffType<SpecialReadyBuff>(), 60 * 60);
            _hudPlayer!.SetOverheadText("Special charged!", 90, color: new Color(255, 155, 0));
            SoundHelper.PlayAudio(SoundPaths.SpecialReady.ToSoundStyle(), 0.6f, maxInstances: 1, position: Player.Center);
        }

        public void UnreadySpecial()
        {
            SpecialReady = false;
            SpecialPoints = 0;
            SpecialActivated = false;
            Player.ClearBuff(ModContent.BuffType<SpecialReadyBuff>());
        }

        private bool TryActivateSpecial()
        {
            if (SpecialActivated || !Player.ItemTimeIsZero)
            {
                return false;
            }

            Item? item = InventoryHelper.FirstInInventory<TrizookaSpecial>(Player);
            if (item != null)
            {
                if (item.ModItem is BaseSpecial special)
                {
                    SpecialDrainRate = special.SpecialDrainPerTick;
                }

                if (item.ModItem is TrizookaSpecial trizooka)
                {
                    ProjectileHelper.CreateProjectileWithWeaponProperties(Player, ModContent.ProjectileType<TrizookaHeldProjectile>(), trizooka, true, null, damage: item.damage, item.knockBack);
                    return true;
                }
            }

            _hudPlayer!.SetOverheadText("No special weapon equipped!", 60);
            SoundHelper.PlayAudio(SoundPaths.EmptyInkTank.ToSoundStyle(), volume: 0.5f);

            return false;
        }

        private void DrainSpecialCharge()
        {
            SpecialPoints -= SpecialDrainRate;
            if (SpecialPoints <= 0)
            {
                SpecialPoints = 0;
                UnreadySpecial();
            }
        }

        private void SpecialDustStream()
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
                    newColor: _colorChipPlayer!.GetColorFromChips(),
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
                newColor: _colorChipPlayer!.GetColorFromChips(),
                Scale: 2f);

                dustInst = Main.dust[dustId];
                dustInst.noGravity = true;
                dustInst.fadeIn = 1.35f;
            }
        }

        // Public methods for other classes
        public void IncrementSpecialCharge(float amount)
        {
            SpecialPoints += amount;
            if (SpecialPoints > SpecialPointsMax) SpecialPoints = SpecialPointsMax;
        }

        public float GetSpecialPercentageDisplay()
        {
            return _specialDisplayPercentage;
        }

        /*
        // Special gauge
        public float SpecialPoints;
        public float SpecialPointsMax = 100;
        public bool SpecialReady;
        public bool IsSpecialActive;
        public string? SpecialName = null;
        public float SpecialDrain;
        public int SpecialIncrementCooldown = 0;
        public int SpecialIncrementCooldownDefault = 6;

        private ColorChipPlayer colorChipPlayer => Player.GetModPlayer<ColorChipPlayer>();

        public override void PreUpdate()
        {
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
                SoundHelper.PlayAudio(SoundPaths.SpecialReady.ToSoundStyle(), volume: 0.8f, pitchVariance: 0.1f, maxInstances: 1);
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

        // Netcode

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
        }

        private void SyncSpecialChargeData()
        {
            var dto = new WeaponPlayerDTO(
                specialReady: SpecialReady);

            SendPacket(dto);
        }*/
    }
}
