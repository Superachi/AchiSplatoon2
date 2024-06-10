using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class SplattershotProjectile : ModProjectile
    {
        private InkColor inkColor = InkColor.Blue;

        private bool visible = false;
        private float delayUntilVisible = 4f;
        private float delayUntilFall = 12f;
        private float fallSpeed = 0.3f;
        private float terminalVelocity = 12f;
        
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = false;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
        {
            var sample = new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/SplattershotShoot");
            var shootSound = sample with
            {
                Volume = 0.2f,
                PitchVariance = 0.1f,
                MaxInstances = 3
            };
            SoundEngine.PlaySound(shootSound);

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

            if (Projectile.ai[0] >= delayUntilVisible)
            {
                Color dustColor = ColorHelper.GenerateInkColor(inkColor);

                if (!visible)
                {
                    visible = true;
                    Projectile.friendly = true;
                    for (int i = 0; i < 10; i++)
                    {
                        float random = Main.rand.NextFloat(-5, 5);
                        float velX = ((Projectile.velocity.X + random) * 0.5f);
                        float velY = ((Projectile.velocity.Y + random) * 0.5f);
                        int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
                    }
                }

                Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Vector2.Zero, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
                for (int i = 0; i < 3; i++)
                {
                    Vector2 spawnPosition = Projectile.oldPosition != Vector2.Zero ? Vector2.Lerp(Projectile.position, Projectile.oldPosition, Main.rand.NextFloat()) : Projectile.position;
                    Dust.NewDustPerfect(Position: spawnPosition, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Projectile.velocity / 5, newColor: dustColor, Scale: 1.2f);
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, System.Int32 damageDone)
        {
            target.AddBuff(103, 180); //On Fire! debuff for 3 seconds
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                float random = Main.rand.NextFloat(-5, 5);
                float velX = ((Projectile.velocity.X + random) * -0.5f);
                float velY = ((Projectile.velocity.Y + random) * -0.5f);
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: ColorHelper.GenerateInkColor(inkColor), Scale: Main.rand.NextFloat(0.8f, 1.6f));
            }
        }
    }
}