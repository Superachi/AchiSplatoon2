using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Projectiles.SpecialProjectiles;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class SpecialPlayer : ModPlayer
    {
        public float SpecialPoints;
        public float SpecialPointsMax = 100;
        public bool SpecialReady;
        public bool SpecialActivated;
        public float SpecialDrainRate;

        public int bossSpecialDropCooldown = 0;
        public int bossSpecialDropCooldownMax = 0;

        private bool _playerCarriesSpecialWeapon;
        public bool PlayerCarriesSpecialWeapon => _playerCarriesSpecialWeapon;

        private ColorChipPlayer? _colorChipPlayer;
        private HudPlayer? _hudPlayer;

        public float SpecialPercentage => MathHelper.Clamp(SpecialPoints / SpecialPointsMax, 0, 1);
        private float _specialDisplayPercentage;
        private float _UIOffsetY = 0;
        private float _UIOffsetYSpeed = 0;

        public override void Initialize()
        {
            _specialDisplayPercentage = 0f;
            _colorChipPlayer = Player.GetModPlayer<ColorChipPlayer>();
            _hudPlayer = Player.GetModPlayer<HudPlayer>();
            _playerCarriesSpecialWeapon = false;

            bossSpecialDropCooldown = 0;
            bossSpecialDropCooldownMax = 120;
        }

        public bool CanIncrementSpecial()
        {
            return _playerCarriesSpecialWeapon && !SpecialReady && !SpecialActivated;
        }

        public override void PreUpdate()
        {
            // UI stuff
            _specialDisplayPercentage = MathHelper.Lerp(_specialDisplayPercentage, SpecialPercentage, 0.2f);
            _playerCarriesSpecialWeapon = InventoryHelper.FirstInInventory<BaseSpecial>(player: Player) != null;

            // Determines at what height to draw the charge bar UI (makes it 'hop' when special charge is collected)
            _UIOffsetY += _UIOffsetYSpeed;
            if (_UIOffsetYSpeed < 4) _UIOffsetYSpeed += 0.5f;
            if (_UIOffsetY > 0) _UIOffsetY = 0;

            if (bossSpecialDropCooldown > 0) bossSpecialDropCooldown--;

            if (SpecialPercentage >= 1 && !SpecialReady)
            {
                ReadySpecial();
            }

            if (SpecialReady && !_playerCarriesSpecialWeapon)
            {
                UnreadySpecial();
                return;
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
            Player.AddBuff(ModContent.BuffType<SpecialReadyBuff>(), 2);
            _hudPlayer!.SetOverheadText("Special charged!", 90, color: new Color(255, 155, 0));
            SoundHelper.PlayAudio(SoundPaths.SpecialReady.ToSoundStyle(), 0.6f, maxInstances: 1, position: Player.Center);
        }

        public void UnreadySpecial()
        {
            SpecialReady = false;
            SpecialActivated = false;
            SpecialPoints = 0;
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

                    var dronePlayer = Player.GetModPlayer<PearlDronePlayer>();
                    dronePlayer.TriggerDialoguePlayerActivatesSpecial(item.type);
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
            if (Main.rand.NextBool(4))
            {
                DustHelper.NewDust(Player.TopLeft + new Vector2(Main.rand.Next(Player.width), -10),
                    dustType: ModContent.DustType<SplatterBulletDust>(),
                    velocity: new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-4, -1)),
                    color: _colorChipPlayer!.GetColorFromChips(),
                    scale: 1.5f,
                    data: new(emitLight: false));
            }

            if (Main.rand.NextBool(20))
            {
                var d = DustHelper.NewDust(
                    position: Player.Center + Main.rand.NextVector2CircularEdge(4, 4),
                    dustType: ModContent.DustType<SplatterBulletDust>(),
                    velocity: Vector2.Zero,
                    color: _colorChipPlayer!.GetColorFromChips(),
                    scale: 2f,
                    data: new(emitLight: false, scaleIncrement: -0.05f, gravity: -0.1f));

                d.velocity = d.position.DirectionTo(Player.Center) * -2;
            }

            if (Main.rand.NextBool(20))
            {
                DustHelper.NewDust(
                    position: Player.TopLeft + new Vector2(Main.rand.Next(Player.width), Main.rand.Next(Player.height)),
                    dustType: DustID.YellowStarDust,
                    velocity: new Vector2(0, Main.rand.NextFloat(-1, -2)),
                    color: Color.White,
                    scale: Main.rand.NextFloat(1f, 2f));
            }
        }

        // Public methods for other classes
        public void IncrementSpecialCharge(float amount)
        {
            var accPlayer = Player.GetModPlayer<AccessoryPlayer>();

            var oldAmount = SpecialPoints;
            SpecialPoints += amount * accPlayer.specialChargeMultiplier;
            if (SpecialPoints > SpecialPointsMax) SpecialPoints = SpecialPointsMax;

            HopChargeUI(SpecialPoints - oldAmount);
        }

        public void ApplyBossSpecialDropCooldown()
        {
            bossSpecialDropCooldown = bossSpecialDropCooldownMax;
        }

        public float GetSpecialPercentageDisplay()
        {
            return _specialDisplayPercentage;
        }

        public float GetChargeUIOffsetY()
        {
            return _UIOffsetY;
        }

        public void HopChargeUI(float pointsGained)
        {
            _UIOffsetY = 0;
            _UIOffsetYSpeed = -MathHelper.Clamp(pointsGained / 2, 1, 4);
        }

        /*
            AddSpecialPointsOnMovement();
            if (Player.dead) return;
            {
                amount *= accMP.specialChargeMultiplier;
                SpecialPoints = Math.Clamp(SpecialPoints + amount, 0, SpecialPointsMax);
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
