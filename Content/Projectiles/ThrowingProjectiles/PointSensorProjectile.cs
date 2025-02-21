using AchiSplatoon2.Content.Buffs.Debuffs;
using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class PointSensorProjectile : BaseBombProjectile
    {
        protected override bool FallThroughPlatforms => true;

        private readonly float indirectHitDamageFalloff = 0.6f;
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;

            DrawOffsetX = -2;
            DrawOriginOffsetY = -12;
        }

        protected float ExplosionTime
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            Projectile.damage = 0;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!hasExploded)
            {
                Projectile.damage = (int)(Projectile.damage * indirectHitDamageFalloff);
                Explode();
            }

            return false;
        }

        private void Explode()
        {
            hasExploded = true;
            Projectile.velocity = Vector2.Zero;

            SoundHelper.PlayAudio(SoundPaths.PointSensorDetonate.ToSoundStyle(), position: Projectile.Center, volume: 0.2f);
            SoundHelper.StopSoundIfActive(throwAudio);

            int loopAmount = 72;
            for (int i = 1; i < loopAmount; i++)
            {
                var offset = new Vector2(finalExplosionRadius * 0.5f, 0);
                offset = WoomyMathHelper.AddRotationToVector2(offset, i * 360 / loopAmount);

                var d = Dust.NewDustDirect(
                    Projectile.Center + offset,
                    0,
                    0,
                    DustID.RainbowTorch,
                    newColor: CurrentColor,
                    Scale: Main.rand.NextFloat(1f, 1.5f));

                d.velocity = d.position.DirectionTo(Projectile.Center) * -16;
                d.velocity = WoomyMathHelper.AddRotationToVector2(d.velocity, 45 * Owner.direction);
                d.noGravity = true;
            }

            for (int i = 0; i < 20; i++)
            {
                var g = Gore.NewGoreDirect(
                    Terraria.Entity.GetSource_None(),
                    Projectile.Center + new Vector2(-20, -15) + Main.rand.NextVector2Circular(finalExplosionRadius * 0.2f, finalExplosionRadius * 0.2f),
                    Vector2.Zero,
                    GoreID.Smoke1,
                    Main.rand.NextFloat(2f, 3f));

                g.velocity = Main.rand.NextVector2Circular(5, 5);
                g.alpha = 220;
            }

            TripleHitDustBurst(Projectile.Center, false);
        }

        public override void AI()
        {
            if (!hasExploded)
            {
                if (FindClosestEnemy(50, false) != null)
                {
                    Explode();
                    return;
                }

                Lighting.AddLight(Projectile.position, CurrentColor.R * brightness, CurrentColor.G * brightness, CurrentColor.B * brightness);

                // Apply air friction
                Projectile.velocity.X = Projectile.velocity.X * airFriction;

                // Rotation increased by velocity.X 
                Projectile.rotation += Projectile.velocity.X * 0.01f;

                // Apply gravity
                if (timeSpentAlive >= delayUntilFall)
                {
                    Projectile.velocity.Y = Math.Clamp(Projectile.velocity.Y + fallSpeed, -terminalVelocity, terminalVelocity);
                }
            }
            else
            {
                ExplosionTime++;

                if ((int)ExplosionTime == 0 || (int)ExplosionTime % 6 == 0)
                {
                    MarkedBuff.ApplyToNpcInRadius(Projectile.Center, finalExplosionRadius, 60 * 10);
                }

                if (timeSpentAlive % 3 == 0)
                {
                    DustHelper.NewDust(
                        Projectile.Center + Main.rand.NextVector2Circular(finalExplosionRadius * 0.8f, finalExplosionRadius * 0.8f),
                        ModContent.DustType<PointSensorPixelDust>(),
                        new Vector2(0, -1),
                        ColorHelper.LerpBetweenColorsPerfect(CurrentColor, Color.White, 0.5f),
                        Main.rand.NextFloat(1f, 2f),
                        new(scaleIncrement: -0.2f, emitLight: false)
                        );
                }
            }

            if (ExplosionTime > 40)
            {
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (hasExploded) return false;

            DrawProjectile(inkColor: CurrentColor, rotation: Projectile.rotation, scale: drawScale, considerWorldLight: false, additiveAmount: 0.5f);
            return false;
        }
    }
}
