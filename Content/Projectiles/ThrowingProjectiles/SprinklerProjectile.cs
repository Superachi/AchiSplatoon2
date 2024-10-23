using AchiSplatoon2.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class SprinklerProjectile : BaseProjectile
    {
        private float delayUntilFall = 12f;
        private float delayUntilDust = 2f;
        private float fallSpeed = 0.3f;
        private float terminalVelocity = 12f;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.ArmorPenetration = 10;
            AIType = ProjectileID.Bullet;
        }

        public override void AfterSpawn()
        {
            Initialize();

            PlayAudio(SoundID.SplashWeak, volume: 0.5f, pitchVariance: 0.3f, maxInstances: 5, pitch: 2f);
            if (Main.rand.NextBool(2))
            {
                PlayAudio(SoundID.SplashWeak, volume: 0.4f, pitchVariance: 0.3f, maxInstances: 5);
            }
            if (Main.rand.NextBool(2))
            {
                PlayAudio(SoundID.Splash, volume: 0.2f, pitchVariance: 0.3f, maxInstances: 5, pitch: 2f);
            }

            var spreadOffset = 0.5f;
            Projectile.velocity.X += Main.rand.NextFloat(-spreadOffset, spreadOffset);
            Projectile.velocity.Y += Main.rand.NextFloat(-spreadOffset, spreadOffset);
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;

            // Start falling eventually
            if (Projectile.ai[0] >= delayUntilFall)
            {
                Projectile.velocity.Y += fallSpeed;

                if (Projectile.velocity.Y >= 0)
                {
                    Projectile.velocity.X *= 0.98f;
                }
            }

            if (Projectile.velocity.Y > terminalVelocity)
            {
                Projectile.velocity.Y = terminalVelocity;
            }

            if (Projectile.ai[0] >= delayUntilDust)
            {
                Color dustColor = GenerateInkColor();
                Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Vector2.Zero, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
                for (int i = 0; i < 3; i++)
                {
                    var dust = Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Projectile.velocity / 5, newColor: dustColor, Scale: 1.2f);
                    dust.alpha = 64;
                }
            }
        }
    }
}
