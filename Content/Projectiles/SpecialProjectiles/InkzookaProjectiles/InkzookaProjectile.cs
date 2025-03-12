using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles.InkzookaProjectiles
{
    internal class InkzookaProjectile : BaseProjectile
    {
        private float fallSpeed = 0f;
        private float fallDelay = 0f;
        private int pierceTimeLeft = 0;
        private bool fading = false;

        private int _originalDirection = 0;

        private float drawAlpha;
        private float drawScale;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 6;

            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = FrameSpeedMultiply(180);
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            ProjectileID.Sets.TrailCacheLength[Type] = FrameSpeedMultiply(5);
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            enablePierceDamagefalloff = false;

            var velocity = Projectile.velocity * GetOwner().direction;
            GetOwner().itemLocation += Vector2.Normalize(velocity) * 3;

            _originalDirection = Math.Sign(Projectile.velocity.X);
            fallSpeed = FrameSpeedDivide(0.1f);
            fallDelay = FrameSpeedMultiply(10f);
            pierceTimeLeft = FrameSpeedMultiply(10);

            drawAlpha = 0f;
            drawScale = 0.5f;
        }

        protected override void AdjustVariablesOnShoot()
        {
            Projectile.velocity *= FrameSpeedDivide(1.5f);
        }

        public override void AI()
        {
            bool CheckSolid()
            {
                return Framing.GetTileSafely(Projectile.Center).HasTile && Collision.SolidCollision(Projectile.Center, 1, 1);
            }

            bool isColliding = CheckSolid();

            if (isColliding)
            {
                pierceTimeLeft--;
                if (pierceTimeLeft <= 0)
                {
                    fading = true;
                    Projectile.friendly = false;
                }

                if (Math.Abs(Projectile.velocity.X) < FrameSpeedDivide(16))
                {
                    Projectile.velocity.X += _originalDirection * FrameSpeedDivide(0.1f);
                }

                if (Projectile.velocity.Y > -1f)
                {
                    Projectile.velocity.Y += -0.1f;
                }
            }
            else
            {
                if (timeSpentAlive > fallDelay)
                {
                    Projectile.velocity.Y += fallSpeed;
                }
            }

            if (fading)
            {
                drawAlpha -= FrameSpeedDivide(0.1f);

                if (drawAlpha <= 0)
                {
                    Projectile.Kill();
                    return;
                }
            }
            else
            {
                if (timeSpentAlive < FrameSpeedMultiply(5))
                {
                    if (drawAlpha < 0.8f)
                    {
                        drawAlpha += FrameSpeedDivide(0.2f);
                    }

                    if (drawScale < 2)
                    {
                        drawScale += FrameSpeedDivide(0.2f);
                    }
                }
            }

            Projectile.rotation = MathHelper.Lerp(Projectile.rotation, Projectile.velocity.X * 0.02f, 0.1f);

            // Frame animation
            if (timeSpentAlive % FrameSpeedMultiply(2) == 0)
            {
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
            }

            // Audio
            if (fading) return;

            if (timeSpentAlive % 30 == 0 && Main.rand.NextBool(2))
            {
                PlayAudio(SoundID.Drown, volume: 0.2f, pitchVariance: 0.3f, pitch: 0.8f, maxInstances: 10);
            }

            if (timeSpentAlive % 10 == 0 && Main.rand.NextBool(5))
            {
                List<SoundStyle> sounds = new()
                {
                    SoundID.Item85,
                    SoundID.Item86,
                    SoundID.Item87,
                };

                PlayAudio(Main.rand.NextFromCollection(sounds), volume: 0.1f, pitchVariance: 0.3f, pitch: 0f, maxInstances: 10, position: Projectile.Center);
            }

            // Dust
            if (isColliding)
            {
                DustHelper.NewDust(
                    position: Projectile.Center + Main.rand.NextVector2Circular(20, 20),
                    dustType: ModContent.DustType<SplatterBulletDust>(),
                    velocity: Projectile.velocity + new Vector2(0, -0.1f),
                    color: ColorHelper.ColorWithAlphaZero(ColorHelper.AddRandomHue(10, CurrentColor)),
                    scale: Main.rand.NextFloat(1f, 2f),
                    data: new(scaleIncrement: -0.1f, gravity: -0.1f));
            }

            if (timeSpentAlive % 12 * FrameSpeed() == 0)
            {
                DustHelper.NewDust(
                    position: Projectile.Center + new Vector2(0, -40) + Main.rand.NextVector2Circular(50, 100),
                    dustType: ModContent.DustType<SplatterBulletDust>(),
                    velocity: -Projectile.velocity / 4,
                    color: ColorHelper.ColorWithAlphaZero(ColorHelper.AddRandomHue(10, CurrentColor)),
                    scale: Main.rand.NextFloat(0.6f, 1.2f),
                    data: new(scaleIncrement: -0.01f, gravity: -0.1f));
            }

            if (timeSpentAlive % 6 * FrameSpeed() == 0)
            {
                DustHelper.NewDust(
                    position: Projectile.Center + new Vector2(0, -40) + Main.rand.NextVector2Circular(50, 100),
                    dustType: ModContent.DustType<SplatterBulletDust>(),
                    velocity: Projectile.velocity / 4,
                    color: ColorHelper.ColorWithAlphaZero(ColorHelper.AddRandomHue(10, CurrentColor)),
                    scale: Main.rand.NextFloat(0.5f, 1f));
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            TripleHitDustBurst(target.Center, false);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.FinalDamage *= WoomyMathHelper.CalculateInkzookaDamageModifier(target);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            var width = 40;
            var height = 160;

            hitbox = new Rectangle(
                x: (int)Projectile.Center.X - width / 2,
                y: (int)Projectile.Center.Y - height,
                width: width,
                height: height);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (timeSpentAlive > 5)
            {
                for (int i = 0; i < 6; i++)
                {
                    var frequency = 25f;
                    var amplitude = 15;
                    var waveDistance = 0.3f;
                    var waveDifference = 30f;
                    var hOffset = (float)Math.Sin((timeSpentAlive + i * waveDifference) / frequency) * amplitude * (i * waveDistance);
                    var vOffset = Math.Min(FrameSpeedDivide(timeSpentAlive), 20);

                    DrawProjectile(
                        CurrentColor.IncreaseHueBy(-(i * 3)),
                        rotation: Projectile.rotation,
                        scale: drawScale * (0.4f + (0.2f * i)),
                        alphaMod: drawAlpha * 0.9f,
                        considerWorldLight: false,
                        positionOffset: Vector2.Zero + new Vector2(hOffset * _originalDirection, vOffset * -i),
                        additiveAmount: 0.6f);
                }
            }

            return false;
        }
    }
}
