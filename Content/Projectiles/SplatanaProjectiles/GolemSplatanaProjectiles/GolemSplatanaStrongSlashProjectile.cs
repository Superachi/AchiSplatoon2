using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Content.EnumsAndConstants;
using System.Collections.Generic;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Content.Players;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles.GolemSplatanaProjectiles
{
    internal class GolemSplatanaStrongSlashProjectile : SplatanaStrongSlashProjectile
    {
        protected override bool ProjectileDust => false;

        private Projectile? meleeProjectile;
        private readonly List<int> _targetsToIgnore = new();

        private int _useTime;
        protected float initialDirection;
        protected int segmentCount;
        private Vector2 _directionToPlayer = Vector2.Zero;
        private float _offsetDegrees;
        private float _angleWithOffset;
        private Vector2 _directionWithOffset;
        private float _sizeMod;

        private float _drawAlpha;
        private float _minimumAlpha;
        private Vector2 _offsetFromPlayer;

        private Color _colorToLerpTo;
        private Color _colorToLerpFrom;
        private Texture2D _sparkleSprite;
        private Texture2D _glowSprite;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 0;
            Projectile.friendly = false;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = FrameCount;
        }

        protected void SetSwingSettings(int swingSegments, float swingOffsetDegrees, float sizeMod)
        {
            segmentCount = swingSegments;
            _offsetDegrees = swingOffsetDegrees;
            _sizeMod = sizeMod;
        }

        protected float ChipVelocityBonus()
        {
            return 1 + Owner.GetModPlayer<ColorChipPlayer>().CalculateProjectileVelocityBonus();
        }

        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            GolemSplatana weaponData = (GolemSplatana)WeaponInstance;
            _useTime = weaponData.UseTime();

            dissolvable = false;
            enablePierceDamagefalloff = false;

            Projectile.localNPCHitCooldown = 5 * FrameSpeed();
            Projectile.velocity = Vector2.Zero;
            _drawAlpha = 0f;
            _minimumAlpha = 0.3f;
            _offsetFromPlayer = Vector2.Zero;

            initialDirection = Owner.direction;
            SetSwingSettings(8, 20, 0.6f * ChipVelocityBonus());

            var random = Main.rand.NextFloat(0f, 1f);
            var lightCol = ColorHelper.LerpBetweenColors(Color.Orange, Color.Yellow, random);
            var darkCol = ColorHelper.LerpBetweenColors(Color.DarkRed, Color.Red, random);
            SetSwingVisuals(lightCol, darkCol, TexturePaths.Medium4pSparkle.ToTexture2D(), TexturePaths.Glow100x.ToTexture2D());
            SlashSound();

            // Find the melee projectile that was spawned by the same owner
            foreach (var projectile in Main.ActiveProjectiles)
            {
                if (projectile.ModProjectile is SplatanaMeleeProjectile
                    && projectile.owner == Projectile.owner)
                {
                    meleeProjectile = projectile;
                    break;
                }
            }
        }

        protected float GetAngleWithOffset()
        {
            return _directionToPlayer.ToRotation() + MathHelper.ToRadians(-_offsetDegrees * initialDirection);
        }

        protected Vector2 GetDirectionWithOffset()
        {
            return _angleWithOffset.ToRotationVector2();
        }

        public override void AI()
        {
            if (Projectile.penetrate != -1) Projectile.penetrate = -1;

            if (_drawAlpha < 0)
            {
                Projectile.Kill();
                return;
            }

            if (meleeProjectile != null && meleeProjectile.active)
            {
                Projectile.Center = meleeProjectile.Center;
                _offsetFromPlayer = Projectile.Center - Owner.Center;
            }
            else
            {
                Projectile.Center = Owner.Center + _offsetFromPlayer;
            }

            _directionToPlayer = Projectile.DirectionTo(Owner.Center);
            _angleWithOffset = GetAngleWithOffset();
            _directionWithOffset = GetDirectionWithOffset();

            for (int i = 1; i < segmentCount; i++)
            {
                var segmentPosition = GetSegmentPosition(i);

                if (_drawAlpha > _minimumAlpha)
                {
                    CreateSwingDust(segmentPosition);
                }
            }

            // Fade in/out
            float attackSpeed = Owner.GetAttackSpeed(DamageClass.Melee);
            if (attackSpeed <= 0) attackSpeed = 0.05f;
            var speedMod = 1 + ((_useTime / attackSpeed) - _useTime) / _useTime;

            if (timeSpentAlive < _useTime * (0.5f * speedMod) && _drawAlpha < 1)
            {
                _drawAlpha += 0.15f / speedMod;
            }

            if (timeSpentAlive > _useTime * (0.8f * speedMod))
            {
                _drawAlpha -= 0.15f / speedMod;
            }

            // DebugHelper.PrintWarning($"{_useTime * speedMod} : {timeSpentAlive > _useTime * speedMod}");
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.friendly) return false;
            if (target.dontTakeDamage) return false;
            if (_targetsToIgnore.Contains(target.whoAmI)) return false;
            if (_drawAlpha < _minimumAlpha) return false;

            bool multiCheck = Collision.CanHitLine(Owner.Center, 2, 2, target.Center, 2, 2) ||
                Collision.CanHitLine(Owner.Top + new Vector2(0, -20), 2, 2, target.Top + new Vector2(0, -20), 2, 2) ||
                Collision.CanHitLine(Owner.Bottom + new Vector2(0, 20), 2, 2, target.Bottom + new Vector2(0, 20), 2, 2);

            if (!multiCheck) return false;

            for (int i = 0; i < segmentCount; i++)
            {
                var positionToCheck = GetSegmentPosition(i);
                int hitboxSize = (int)(160 * _sizeMod);
                var rectangleToCheck = new Rectangle((int)positionToCheck.X - hitboxSize / 2, (int)positionToCheck.Y - hitboxSize / 2, hitboxSize, hitboxSize);

                if (Rectangle.Intersect(target.Hitbox, rectangleToCheck) != Rectangle.Empty)
                {
                    _targetsToIgnore.Add(target.whoAmI);

                    HitTarget(target);
                    CreateHitVisual(target.Center);

                    return true;
                }
            }

            return false;
        }

        protected void SetSwingVisuals(Color colorToLerpFrom, Color colorToLerpTo, Texture2D sparkleSprite, Texture2D glowSprite)
        {
            _colorToLerpFrom = colorToLerpFrom;
            _colorToLerpTo = colorToLerpTo;
            _sparkleSprite = sparkleSprite;
            _glowSprite = glowSprite;
        }

        public override void PostDraw(Color lightColor)
        {
            for (int i = segmentCount - 1; i > 0; i--)
            {
                // Big background slash
                DrawProjectile(
                    ColorHelper.LerpBetweenColorsPerfect(_colorToLerpFrom, _colorToLerpTo, 1f - i / (float)segmentCount),
                    rotation: _directionWithOffset.ToRotation(),
                    scale: (1 + i * 0.5f) * _sizeMod * -initialDirection,
                    alphaMod: _drawAlpha * (i * 0.05f / segmentCount + 0.2f),
                    additiveAmount: 0.4f + 0.2f * i,
                    considerWorldLight: false,
                    positionOffset: _directionWithOffset * i * (-20 * _sizeMod - 20),
                    flipSpriteSettings: (SpriteEffects)(initialDirection == -1 ? 1 : 0),
                    spriteOverride: TexturePaths.GolemSlashSegment.ToTexture2D());

                // Smaller, brighter slash
                DrawProjectile(
                    ColorHelper.LerpBetweenColorsPerfect(_colorToLerpFrom, _colorToLerpTo, 1f - i / (float)segmentCount),
                    rotation: _directionWithOffset.ToRotation(),
                    scale: (1 + i * 0.5f) * _sizeMod * -initialDirection * 0.6f,
                    alphaMod: _drawAlpha * (i * 0.02f / segmentCount + 0.2f),
                    additiveAmount: 1f + 0.5f * i,
                    considerWorldLight: false,
                    positionOffset: _directionWithOffset * i * (-20 * _sizeMod - 20),
                    flipSpriteSettings: (SpriteEffects)(initialDirection == -1 ? 1 : 0),
                    spriteOverride: TexturePaths.GolemSlashSegment.ToTexture2D());

                if (i == segmentCount - 1)
                {
                    SpriteBatch spriteBatch = Main.spriteBatch;

                    float scale = _drawAlpha * 0.5f;

                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                    var jCount = 12;
                    for (int j = 0; j < jCount; j++)
                    {
                        if (j % 4 == 0) continue;

                        var lerpColor = ColorHelper.LerpBetweenColorsPerfect(_colorToLerpFrom, _colorToLerpTo, (float)j / (float)jCount);
                        Color color = ColorHelper.ColorWithAlpha255(lerpColor) * _drawAlpha * 0.3f * j;

                        Main.EntitySpriteDraw(
                            texture: _sparkleSprite,
                            position: GetSegmentPosition(i + 1f) - Main.screenPosition,
                            sourceRectangle: null,
                            color: color,
                            rotation: 0,
                            origin: _sparkleSprite.Size() / 2,
                            scale: scale * 0.05f * j * segmentCount,
                            effects: SpriteEffects.None);
                    }

                    var colorBetween = ColorHelper.LerpBetweenColorsPerfect(_colorToLerpFrom, _colorToLerpTo, 0.5f);

                    Main.EntitySpriteDraw(
                        texture: _glowSprite,
                        position: GetSegmentPosition(i + 1f) - Main.screenPosition,
                        sourceRectangle: null,
                        color: ColorHelper.ColorWithAlpha255(colorBetween) * _drawAlpha,
                        rotation: 0,
                        origin: _glowSprite.Size() / 2,
                        scale: 3,
                        effects: SpriteEffects.None);

                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }
        }

        protected virtual void SlashSound()
        {
            PlayAudio(SoundID.Item7, volume: 1f, pitchVariance: 0.3f, pitch: 0f, maxInstances: 10);
            PlayAudio(SoundID.Item18, volume: 1f, pitchVariance: 0f, pitch: 0f, maxInstances: 10);

            PlayAudio(SoundID.Item66, volume: 0.5f, pitchVariance: 0f, pitch: 0f, maxInstances: 10);
            PlayAudio(SoundID.DD2_BetsysWrathShot, volume: 0.1f, pitchVariance: 0f, pitch: 0f, maxInstances: 10);
            PlayAudio(SoundID.DD2_PhantomPhoenixShot, volume: 1f, pitchVariance: 0f, pitch: 0.5f, maxInstances: 10);
            PlayAudio(SoundID.DD2_DrakinShot, volume: 0.3f, pitchVariance: 0f, pitch: 0.5f, maxInstances: 10);
        }

        protected Vector2 GetSegmentPosition(float segmentNumber)
        {
            return Projectile.Center + _directionWithOffset * segmentNumber * (-20 * _sizeMod - 20);
        }

        protected virtual void CreateHitVisual(Vector2 position)
        {
            PlayAudio(SoundID.Item14, volume: 0.5f, pitchVariance: 0.2f, pitch: -0.5f, maxInstances: 5);

            for (int j = 0; j < 15; j++)
            {
                DustHelper.NewDust(
                    position,
                    DustID.Torch,
                    Main.rand.NextVector2Circular(16, 16),
                    ColorHelper.LerpBetweenColors(Color.Red, Color.Orange, Main.rand.NextFloat(0f, 1f)),
                    Main.rand.NextFloat(1f, 3f));
            }

            for (int j = 0; j < 3; j++)
            {
                var g = Gore.NewGoreDirect(
                    Terraria.Entity.GetSource_None(),
                    position,
                    Vector2.Zero,
                    GoreID.Smoke1,
                    Main.rand.NextFloat(1f, 2f));

                g.velocity = Main.rand.NextVector2Circular(3, 3);
                g.alpha = 196;
            }
        }

        protected virtual void HitTarget(NPC target)
        {
            var proj = CreateChildProjectile<HitProjectile>(target.Center, Vector2.Zero, Projectile.damage, true);
            proj.targetToHit = target.whoAmI;
            proj.immuneTime = 3;

            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.OnFire3, 300);
            }
        }

        protected virtual void CreateSwingDust(Vector2 position)
        {
            if (timeSpentAlive % FrameSpeedMultiply(2) == 0)
            {
                DustHelper.NewDust(
                    position + Main.rand.NextVector2Circular(40, 40),
                    DustID.Torch,
                    WoomyMathHelper.AddRotationToVector2(_directionWithOffset, 90) * Main.rand.NextFloat(1, 6) * -initialDirection,
                    Main.rand.Next([Color.Purple, Color.Red, Color.Orange]),
                    Main.rand.NextFloat(0.5f, 3f));
            }

            if (Main.rand.NextBool(10))
            {
                var d = DustHelper.NewDust(
                    position + Main.rand.NextVector2Circular(40, 40),
                    DustID.Smoke,
                    WoomyMathHelper.AddRotationToVector2(_directionWithOffset, 90) * Main.rand.NextFloat(1, 3) * -initialDirection,
                    ColorHelper.LerpBetweenColors(Color.Black, Color.Gray, 0.3f),
                    Main.rand.NextFloat(0.5f, 1f));

                d.noGravity = false;
            }
        }
    }
}
