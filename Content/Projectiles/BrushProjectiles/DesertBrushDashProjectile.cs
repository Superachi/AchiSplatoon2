using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Projectiles.ProjectileVisuals;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria;
using System.Collections.Generic;
using AchiSplatoon2.Content.Players;

namespace AchiSplatoon2.Content.Projectiles.BrushProjectiles
{
    internal class DesertBrushDashProjectile : BaseProjectile
    {
        protected override float DamageModifierAfterPierce => 1f;

        private float _drawAlpha;
        private float _drawScale;
        private float _drawRotation;
        private float _brightness;

        private const int _stateSpawn = 0;
        private const int _stateFollowPlayer = 1;
        private const int _stateLaunch = 2;
        private const int _stateDisappear = 3;

        private float _charge;
        private int _maxCharge;
        private bool _isChargeMaxed;
        private float _chargedAttackInkCost;
        private int _hitboxSize;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = false;
            Projectile.timeLeft = 60000;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            ProjectileID.Sets.TrailCacheLength[Type] = 4;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseBrush;
            _chargedAttackInkCost = WoomyMathHelper.CalculateWeaponInkCost(WeaponInstance, Owner) * 5;
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            ApplyWeaponInstanceData();

            enablePierceDamagefalloff = false;

            _maxCharge = 100;
            _hitboxSize = 50;

            // Set visuals
            _brightness = 0.005f;
            _drawRotation = MathHelper.ToRadians(Main.rand.Next(0, 359));
            _drawScale = 2;
            _drawAlpha = 0;

            SetState(_stateSpawn);
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overWiresUI.Add(index);
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);
            switch (state)
            {
                case _stateLaunch:
                case _stateDisappear:
                    if (parentProjectile != null
                        && parentProjectile.ModProjectile is DesertBrushSwingProjectile parent)
                    {
                        parent.ChildProjectile = null;
                    }
                    break;
            }

            switch (state)
            {
                case _stateFollowPlayer:
                    Projectile.friendly = true;

                    break;

                case _stateLaunch:
                    SoundHelper.PlayAudio(SoundID.DD2_WyvernDiveDown, volume: 0.5f, pitch: 0.5f, pitchVariance: 0.2f, position: Projectile.Center);
                    SoundHelper.PlayAudio(SoundID.DD2_WyvernDiveDown, volume: 0.5f, pitch: 0.5f, pitchVariance: 0.2f, position: Projectile.Center);

                    break;

                case _stateDisappear:
                    SoundHelper.PlayAudio(SoundID.Item109, volume: 0.3f, pitchVariance: 0.9f, position: Projectile.Center);

                    break;
            }
        }

        public override void AI()
        {
            var rotateDirection = state == _stateLaunch ? Math.Sign(Projectile.velocity.X) : 1;
            var rotateSpeed = rotateDirection * 0.4f;
            if (state == _stateLaunch) rotateSpeed *= 1.5f;
            if (state == _stateDisappear) rotateSpeed *= 0.5f;

            _drawRotation += rotateSpeed;

            switch (state)
            {
                case _stateSpawn:
                    Projectile.Center = Owner.Center + new Vector2(0, Owner.gfxOffY);
                    var scaleGoal = 0.5f;
                    _drawScale = MathHelper.Lerp(_drawScale, scaleGoal, 0.2f);

                    if (_drawAlpha < 1f)
                    {
                        _drawAlpha += 0.1f;
                    }

                    if (timeSpentInState > 20)
                    {
                        _drawScale = scaleGoal;
                        _drawAlpha = 1f;
                        AdvanceState();
                    }

                    break;

                case _stateFollowPlayer:
                    Projectile.Center = Owner.Center + new Vector2(0, Owner.gfxOffY);

                    _hitboxSize = 50 + (int)(50 * ChargeQuotient());
                    _drawScale = _hitboxSize / 100f;

                    if (_charge < FrameSpeedMultiply(_maxCharge))
                    {
                        var increment = Owner.velocity.Length() / 2;
                        _charge += increment;
                        // UpdateCurrentColor(CurrentColor.IncreaseHueBy(increment / 2));
                    }
                    else if (!_isChargeMaxed)
                    {
                        _isChargeMaxed = true;
                        SoundHelper.PlayAudio(SoundID.Item4, volume: 0.2f, position: Projectile.Center);
                        CreateChildProjectile<WeaponChargeSparkleVisual>(Owner.Center, Vector2.Zero, 0, true);
                    }

                    var inkTankPlayer = GetOwnerModPlayer<InkTankPlayer>();
                    float inkCost = _chargedAttackInkCost * ChargeQuotient();

                    bool hasEnoughInk = inkTankPlayer.HasEnoughInk(inkCost, false);

                    if (!GetOwnerModPlayer<WeaponPlayer>().isBrushRolling)
                    {
                        if (_isChargeMaxed && hasEnoughInk && !Owner.dead)
                        {
                            ConsumeInk(inkCost, consumeInkAsChildProj: true);
                            Projectile.velocity = Owner.DirectionTo(Main.MouseWorld) * (1 + ChargeQuotient()) * 12;

                            AdvanceState();
                        }
                        else
                        {
                            if (!hasEnoughInk)
                            {
                                inkTankPlayer.CreateLowInkPopup();
                            }
                            SetState(_stateDisappear);
                        }
                    }

                    break;

                case _stateLaunch:
                    if (Projectile.velocity.Length() > 3f)
                    {
                        Projectile.velocity *= 0.97f;
                    }

                    if (timeSpentInState > FrameSpeedMultiply(60) && _drawAlpha > 0f)
                    {
                        _drawAlpha -= 0.1f;

                        if (_drawAlpha <= 0.5f)
                        {
                            Projectile.friendly = false;
                        }
                    }

                    if (_drawAlpha <= 0.05f)
                    {
                        Projectile.Kill();
                        return;
                    }

                    void CreateDust(Vector2 position)
                    {
                        var d = DustHelper.NewDust(
                            position: position - Main.rand.NextVector2Circular(10, 10),
                            dustType: DustID.WhiteTorch,
                            velocity: Projectile.velocity,
                            color: ColorHelper.AddRandomHue(15, CurrentColor),
                            scale: 2);
                        d.noLight = true;
                    }

                    var degreesOffset = Main.rand.Next(30, 150);
                    if (Main.rand.NextBool(10))
                    {
                        var position = Projectile.Center + WoomyMathHelper.AddRotationToVector2(new Vector2(_hitboxSize / 2, 0), MathHelper.ToDegrees(Projectile.velocity.ToRotation()) + degreesOffset);
                        CreateDust(position);
                    }

                    if (Main.rand.NextBool(10))
                    {
                        var position = Projectile.Center + WoomyMathHelper.AddRotationToVector2(new Vector2(_hitboxSize / 2, 0), MathHelper.ToDegrees(Projectile.velocity.ToRotation()) - degreesOffset);
                        CreateDust(position);
                    }

                    break;

                case _stateDisappear:
                    Projectile.Center = Owner.Center + new Vector2(0, Owner.gfxOffY);
                    _drawScale += 0.05f;

                    if (_drawAlpha > 0f)
                    {
                        _drawAlpha -= 0.05f;
                    }

                    if (_drawAlpha <= 0f)
                    {
                        Projectile.Kill();
                        return;
                    }

                    break;
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            SetHitboxSize(_hitboxSize, out hitbox);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ProjectileBounce(oldVelocity, new Vector2(0.5f, 0.5f));
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.DisableCrit();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var sparkle = CreateChildProjectile<StillSparkleVisual>(target.Center, Vector2.Zero, 0, true);
            sparkle.UpdateCurrentColor(CurrentColor);
            sparkle.AdjustRotation(MathHelper.ToRadians(45));
            sparkle.AdjustScale(1.5f);

            for (int i = 0; i < 8; i++)
            {
                var d = Dust.NewDustPerfect(
                    Position: target.Center,
                    Type: DustID.WhiteTorch,
                    Velocity: Main.rand.NextVector2Circular(10, 10),
                    newColor: ColorHelper.AddRandomHue(15, CurrentColor),
                    Scale: Main.rand.NextFloat(2f, 4f)
                );

                d.color = CurrentColor;
                d.noGravity = true;
                d.noLight = true;
            }

            PlayAudio(SoundID.Item25, volume: 0.5f, pitchVariance: 0.3f, pitch: 0.5f, maxInstances: 1);

            base.OnHitNPC(target, hit, damageDone);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < 6; i++)
            {
                var rotation = _drawRotation + i * 0.3f;

                DrawProjectile(
                    inkColor: ColorHelper.LerpBetweenColorsPerfect(CurrentColor, Color.White, i * 0.2f),
                    rotation: rotation,
                    scale: _drawScale * (1 - i * 0.01f),
                    alphaMod: _drawAlpha * (0.6f - i * 0.05f),
                    considerWorldLight: false,
                    additiveAmount: 1f,
                    originOffset: new Vector2(-10, 0));
            }

            //var len = 2;
            //if (state == _stateLaunch)
            //{
            //    len = Projectile.oldPos.Length;
            //}

            //for (int j = len - 1; j > 0; j--)
            //{
            //    var iMult = (float)(len - j) / len;
            //    var posDiff = Projectile.oldPos[j] - Projectile.position;
            //    var alphaMod = iMult * _drawAlpha;

            //    for (int i = 0; i < 6; i++)
            //    {
            //        var rotation = _drawRotation + i * 0.3f;

            //        DrawProjectile(
            //            inkColor: ColorHelper.LerpBetweenColorsPerfect(CurrentColor, Color.White, i * 0.2f),
            //            rotation: rotation,
            //            scale: _drawScale * (1 - j * 0.1f),
            //            alphaMod: alphaMod * (0.6f - i * 0.05f),
            //            considerWorldLight: false,
            //            additiveAmount: 1f,
            //            originOffset: new Vector2(-2, 0),
            //            positionOffset: posDiff);
            //    }
            //}

            return false;
        }

        private float ChargeQuotient()
        {
            return (float)_charge / (float)_maxCharge;
        }

        private void ChargeUpDustStream()
        {
        }
    }
}
