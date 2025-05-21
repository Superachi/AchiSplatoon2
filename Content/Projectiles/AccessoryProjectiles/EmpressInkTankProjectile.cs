using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Accessories.InkTanks;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.AccessoryProjectiles
{
    internal class EmpressInkTankProjectile : BaseProjectile
    {
        public NPC? target;
        private float _turnWideness;
        private float _flashScale;

        private const int _stateFindTarget = 1;
        private const int _stateChase = 2;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = false;
            Projectile.extraUpdates = 8;
            Projectile.timeLeft = FrameSpeedMultiply(600);
            Projectile.tileCollide = false;

            ProjectileID.Sets.TrailCacheLength[Type] = 14;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public void SetDamageMult(float damageMultiplier)
        {
            Projectile.damage = (int)(EmpressInkTank.ProjectileDamage * damageMultiplier);
        }

        protected override void AfterSpawn()
        {
            SetState(_stateFindTarget);
            Projectile.damage = EmpressInkTank.ProjectileDamage;

            target = FindClosestEnemy(800, false, Owner.Center);

            if (target == null || !target.active)
            {
                Projectile.Kill();
                return;
            }

            _turnWideness = 200f;
            Projectile.velocity = -Projectile.DirectionTo(target.Center) * 3;
            Projectile.velocity = WoomyMathHelper.AddRotationToVector2(Projectile.velocity, -45, 45);
            dissolvable = false;

            // Visuals/sound
            UpdateCurrentColor(Main.DiscoColor);
            _flashScale = 1.6f;

            PlayAudio(SoundID.Item131, volume: 0.2f, pitchVariance: 0.3f, maxInstances: 5, position: Projectile.Center);
            PlayAudio(SoundID.Item45, volume: 0.5f, pitch: 0.2f, pitchVariance: 0.2f, maxInstances: 5, position: Projectile.Center);
            PlayAudio(SoundID.Item28, volume: 0.3f, pitch: 1f, pitchVariance: 0.2f, maxInstances: 5, position: Projectile.Center);
            PlayAudio(SoundID.Item101, volume: 0.3f, pitch: 1f, pitchVariance: 0.2f, maxInstances: 5, position: Projectile.Center);
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);
        }

        public override void AI()
        {
            switch (state)
            {
                case _stateFindTarget:
                    Projectile.velocity *= 0.99f;

                    if (Projectile.velocity.Length() < 0.001f)
                    {
                        Projectile.Kill();
                        return;
                    }

                    if (timeSpentAlive % FrameSpeedMultiply(6) == 0)
                    {
                        target = FindClosestEnemy(800, false, Owner.Center);

                        if (target != null && target.active)
                        {
                            SetState(_stateChase);
                        }
                    }

                    break;
                case _stateChase:
                    if (target == null || !target.active)
                    {
                        SetState(_stateFindTarget);
                        return;
                    }

                    var goalPosition = target.Center;

                    Projectile.velocity += Projectile.DirectionTo(goalPosition) / _turnWideness;

                    if (_turnWideness > 1)
                    {
                        if (timeSpentAlive < FrameSpeedMultiply(30))
                        {
                            _turnWideness -= 0.5f / FrameSpeed();
                        }
                        else
                        {
                            _turnWideness -= 3f / FrameSpeed();
                        }
                    }

                    if (Projectile.velocity.Length() > 3)
                    {
                        Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 2;
                    }

                    break;
            }

            if (float.IsNaN(Projectile.position.X) || float.IsNaN(Projectile.position.Y))
            {
                DebugHelper.PrintError("My position is NaN");
            }

            if (float.IsNaN(Projectile.velocity.X) || float.IsNaN(Projectile.velocity.Y))
            {
                DebugHelper.PrintError("My velocity is NaN");
            }

            if (!Projectile.friendly && FrameSpeedMultiply(30) > 30)
            {
                Projectile.friendly = true;
            }

            // Visuals
            if (_flashScale > 0)
            {
                _flashScale = MathHelper.Lerp(_flashScale, 0, 0.01f);
            }

            if (timeSpentAlive % 3 == 0)
            {
                UpdateCurrentColor(CurrentColor.IncreaseHueBy(1));
            }

            if (timeSpentAlive > FrameSpeedMultiply(1))
            {
                var d = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: DustID.PortalBolt,
                    Velocity: Vector2.Zero,
                    newColor: CurrentColor,
                    Scale: 1f);
                d.noGravity = true;
                d.noLight = true;
            }

            if (timeSpentAlive % 6 == 0 && Main.rand.NextBool(10))
            {
                var d = Dust.NewDustPerfect(
                    Position: Projectile.Center + Main.rand.NextVector2Circular(10, 10),
                    Type: DustID.AncientLight,
                    Velocity: new Vector2(0, Main.rand.NextFloat(-2, -6)),
                    newColor: ColorHelper.LerpBetweenColors(CurrentColor, Color.White, 0.4f),
                    Scale: 1.5f);
                d.noGravity = true;
                d.noLight = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.DisableCrit();

            if (target.type != NPCID.DungeonGuardian)
            {
                modifiers.Defense *= 0;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            for (int i = 0; i < 5; i++)
            {
                var d = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: DustID.SnowSpray,
                    Velocity: Main.rand.NextVector2CircularEdge(10, 10),
                    newColor: CurrentColor,
                    Scale: Main.rand.NextFloat(1f, 2f));
                d.noGravity = true;
                d.noLight = true;
            }
        }

        public override void PostDraw(Color lightColor)
        {
            // Draw layered sparkle + glow
            SpriteBatch spriteBatch = Main.spriteBatch;

            var sparkleSprite = TexturePaths.Medium4pSparkle.ToTexture2D();
            var glowSprite = TexturePaths.Glow100x.ToTexture2D();

            float scale = 0.5f + (float)Math.Sin(MathHelper.ToRadians(timeSpentAlive * 2)) * 0.1f + _flashScale;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(
                texture: sparkleSprite,
                position: Projectile.Center - Main.screenPosition,
                sourceRectangle: null,
                color: ColorHelper.ColorWithAlpha255(Color.White),
                rotation: 0,
                origin: sparkleSprite.Size() / 2,
                scale: scale * 0.6f,
                effects: SpriteEffects.None);

            Main.EntitySpriteDraw(
                texture: sparkleSprite,
                position: Projectile.Center - Main.screenPosition,
                sourceRectangle: null,
                color: ColorHelper.ColorWithAlpha255(CurrentColor),
                rotation: 0,
                origin: sparkleSprite.Size() / 2,
                scale: scale * 1.2f,
                effects: SpriteEffects.None);

            Main.EntitySpriteDraw(
                texture: glowSprite,
                position: Projectile.Center - Main.screenPosition,
                sourceRectangle: null,
                color: ColorHelper.ColorWithAlpha255(CurrentColor) * 0.5f,
                rotation: 0,
                origin: glowSprite.Size() / 2,
                scale: scale * 1.5f,
                effects: SpriteEffects.None);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
