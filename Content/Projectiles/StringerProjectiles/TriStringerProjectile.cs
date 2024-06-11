using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class TriStringerProjectile : BaseProjectile
    {
        private float delayUntilFall = 5f;
        private float fallSpeed = 0.001f;
        private float terminalVelocity = 1f;
        private bool sticking = false;
        private bool hasExpldoded = false;
        protected virtual bool CanStick { get => true; }
        protected virtual int ExplosionRadius { get => 120; }

        public override void SetDefaults()
        {
            Initialize(color: InkColor.Blue, visible: false, visibleDelay: 4f);

            Projectile.light = 0.2f;
            Projectile.extraUpdates = 30;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = ExtraUpdatesTime(240);
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        private float ExtraUpdatesTime(float input)
        {
            return input * Projectile.extraUpdates;
        }

        private int ExtraUpdatesTime(int input)
        {
            return input * Projectile.extraUpdates;
        }

        private void EmitBurstDust(float dustMaxVelocity = 1, int amount = 1, float minScale = 0.5f, float maxScale = 1f, float radiusModifier = 100f)
        {
            float radiusMult = radiusModifier / 100;
            amount = Convert.ToInt32(amount * radiusMult);

            // Ink
            for (int i = 0; i < amount; i++)
            {
                Color dustColor = ColorHelper.GenerateInkColor(inkColor);

                var dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<BlasterExplosionDust>(),
                    new Vector2(Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity), Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity)),
                    0, dustColor, Main.rand.NextFloat(minScale, maxScale));
                dust.velocity *= radiusMult;
            }

            // Firework
            for (int i = 0; i < amount / 2; i++)
            {
                Color dustColor = ColorHelper.GenerateInkColor(inkColor);
                var dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<BlasterExplosionDust>(),
                    new Vector2(Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity), Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity)),
                    0, dustColor);
                dust.velocity *= radiusMult / 2;
            }
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.ai[0] += 1f;

            if (!sticking)
            {
                if (Projectile.ai[0] >= ExtraUpdatesTime(delayUntilFall))
                {
                    Projectile.velocity.Y += fallSpeed;
                }
                if (Projectile.velocity.Y > terminalVelocity)
                {
                    Projectile.velocity.Y = terminalVelocity;
                }

                Color dustColor = ColorHelper.GenerateInkColor(inkColor);
                Dust.NewDustPerfect(Position: Projectile.Center, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Vector2.Zero, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
            } else
            {
                if (Projectile.timeLeft < ExtraUpdatesTime(3) && !hasExpldoded)
                {
                    hasExpldoded = true;
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Projectile.alpha = 255;
                        Projectile.Resize(ExplosionRadius, ExplosionRadius);
                        EmitBurstDust(dustMaxVelocity: 15f, amount: 20, radiusModifier: ExplosionRadius);

                        SoundEngine.PlaySound(
                            new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/BlasterExplosionLight")
                            with {
                                Volume = 0.1f,
                                PitchVariance = 0.1f,
                                MaxInstances = 10
                            });
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (CanStick)
            {
                if (!sticking)
                {
                    sticking = true;
                    Projectile.tileCollide = false;
                    Projectile.position = Projectile.oldPosition;
                    Projectile.velocity = Projectile.velocity * 0.0001f;
                    Projectile.timeLeft = ExtraUpdatesTime(115 + (int)Projectile.ai[2] * 5);
                }
            } else
            {
                for (int i = 0; i < 5; i++)
                {
                    float random = Main.rand.NextFloat(-2, 2);
                    float velX = (Projectile.velocity.X + random) * -0.5f;
                    float velY = (Projectile.velocity.Y + random) * -0.5f;
                    int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: ColorHelper.GenerateInkColor(inkColor), Scale: Main.rand.NextFloat(0.8f, 1.6f));
                }
                Projectile.Kill();
            }

            return false;
        }
    }
}