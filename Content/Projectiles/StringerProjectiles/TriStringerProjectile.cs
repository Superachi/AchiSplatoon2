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
        private float delayUntilFall = 12f;
        private float fallSpeed = 0.002f;
        private float terminalVelocity = 1f;
        private bool sticking = false;
        private bool hasExploded = false;
        protected virtual bool CanStick { get => true; }
        protected virtual int ExplosionRadius { get => 120; }

        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.extraUpdates = 30;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = ExtraUpdatesTime(120);
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Initialize();
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
                var dust = Dust.NewDustPerfect(Projectile.Center, DustID.FireworksRGB,
                    new Vector2(Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity), Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity)),
                    0, dustColor);
                dust.velocity *= radiusMult / 2;
            }
        }

        private void Explode()
        {
            hasExploded = true;
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.alpha = 255;
                Projectile.Resize(ExplosionRadius, ExplosionRadius);
                EmitBurstDust(dustMaxVelocity: 15f, amount: 20, minScale: 1, maxScale: 2, radiusModifier: ExplosionRadius);
                PlayAudio("BlasterExplosionLight", volume: 0.1f, pitchVariance: 0.2f, maxInstances: 10);
            }
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;

            if (!sticking)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
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
                if (Math.Abs(Projectile.velocity.X) > 0.0001f || Math.Abs(Projectile.velocity.Y) > 0.0001f)
                {
                    Projectile.velocity = Projectile.velocity * 0.9f;
                }

                Lighting.AddLight(Projectile.Center, ColorHelper.GenerateInkColor(inkColor).ToVector3());

                if (Projectile.timeLeft < ExtraUpdatesTime(3) && !hasExploded)
                {
                    Explode();
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (CanStick)
            {
                if (!sticking)
                {
                    Projectile.alpha = 0;
                    PlayAudio("InkHitSplash00", volume: 0.2f, pitchVariance: 0.3f, maxInstances: 9);
                    sticking = true;
                    Projectile.tileCollide = false;
                    Projectile.velocity = oldVelocity;
                    Projectile.timeLeft = ExtraUpdatesTime(60 + (int)Projectile.ai[2] * 5);
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (sticking && !hasExploded)
            {
                Explode();
            }
            base.OnHitNPC(target, hit, damageDone);
        }
    }
}