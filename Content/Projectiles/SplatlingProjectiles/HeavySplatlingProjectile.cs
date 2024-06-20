using AchiSplatoon2.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SplatlingProjectiles
{
    internal class HeavySplatlingProjectile : BaseProjectile
    {
        private float delayUntilFall = 20f;
        private float fallSpeed = 0.1f;
        private float terminalVelocity = 10f;

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 3;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Initialize(ignoreDamageMods: true);
            PlayAudio("SplatlingShoot", volume: 0.2f, pitchVariance: 0.2f, maxInstances: 3);

            var spreadOffset = 0.5f;
            Projectile.velocity.X += Main.rand.NextFloat(-spreadOffset, spreadOffset);
            Projectile.velocity.Y += Main.rand.NextFloat(-spreadOffset, spreadOffset);
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;

            // Start falling eventually
            if (Projectile.ai[0] >= delayUntilFall * FrameSpeed())
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

            Color dustColor = GenerateInkColor();
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Vector2.Zero, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
            for (int i = 0; i < 3; i++)
            {
                // Vector2 spawnPosition = Projectile.oldPosition != Vector2.Zero ? Vector2.Lerp(Projectile.position, Projectile.oldPosition, Main.rand.NextFloat()) : Projectile.position;
                var dust = Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Projectile.velocity / 5, newColor: dustColor, Scale: 1.2f);
                dust.alpha = 64;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 5; i++)
            {
                float random = Main.rand.NextFloat(-2, 2);
                float velX = (Projectile.velocity.X + random) * -0.5f;
                float velY = (Projectile.velocity.Y + random) * -0.5f;
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: GenerateInkColor(), Scale: Main.rand.NextFloat(0.8f, 1.6f));
            }
            return true;
        }
    }
}