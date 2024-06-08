using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using static System.Net.Mime.MediaTypeNames;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class BlasterProjectile : ModProjectile
    {
        private const int addedUpdate = 2;
        private const int explosionRadiusAir = 160;
        private const int explosionRadiusTile = 80;
        private const float explosionTime = 6f;
        private const float explosionDelay = 20f * addedUpdate;
        private int state = 0;

        public override void SetDefaults()
        {
            Projectile.light = 1f;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = addedUpdate;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
        {
            var sample = new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/BlasterShoot"); 
            var shootSound = sample with
            {
                Volume = 0.2f,
                PitchVariance = 0.1f,
                MaxInstances = 3
            };
            SoundEngine.PlaySound(shootSound);
        }

        private void ExplodeBig()
        {
            // Audio
            var soundStyle = new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/BlasterExplosion");
            var explosionSound = soundStyle with
            {
                Volume = 0.2f,
                PitchVariance = 0.1f,
                MaxInstances = 3
            };
            SoundEngine.PlaySound(explosionSound);

            // Visual
            int dustMaxVelocity = 10;
            for (int i = 0; i < 30; i++)
            {
                int explosionDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Firework_Pink,
                    Main.rand.Next(-dustMaxVelocity, dustMaxVelocity), Main.rand.Next(-dustMaxVelocity, dustMaxVelocity));
            }

            // Gameplay
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.tileCollide = false;
                Projectile.Resize(explosionRadiusAir, explosionRadiusAir);
                Projectile.velocity = Vector2.Zero;
            }
        }

        private void ExplodeSmall()
        {
            // Audio
            var soundStyle = new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/BlasterExplosionLight");
            var explosionSound = soundStyle with
            {
                Volume = 0.1f,
                PitchVariance = 0.1f,
                MaxInstances = 3
            };
            SoundEngine.PlaySound(explosionSound);

            // Visual
            int dustMaxVelocity = 5;
            for (int i = 0; i < 15; i++)
            {
                int explosionDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Firework_Pink,
                    Main.rand.Next(-dustMaxVelocity, dustMaxVelocity), Main.rand.Next(-dustMaxVelocity, dustMaxVelocity));
            }

            // Gameplay
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.damage /= 2;
                Projectile.tileCollide = false;
                Projectile.Resize(explosionRadiusTile, explosionRadiusTile);
                Projectile.velocity = Vector2.Zero;
            }
        }

        private void AdvanceState()
        {
            state++;
            Projectile.ai[0] = 0;
        }

        private void SetState(int stateId)
        {
            state = stateId;
            Projectile.ai[0] = 0;
        }

        public override void AI()
        {
            // Face direction
            Projectile.rotation = Projectile.velocity.ToRotation();

            switch (state)
            {
                case 0:
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Firework_Pink, Projectile.velocity.X / 2, Projectile.velocity.Y / 2);
                    if (Projectile.ai[0] >= explosionDelay)
                    {
                        ExplodeBig();
                        AdvanceState();
                    }
                    break;
                case 1:
                    if (Projectile.ai[0] >= explosionTime)
                    {
                        Projectile.Kill();
                    }
                    break;
                case 2:
                    if (Projectile.ai[0] >= explosionTime)
                    {
                        Projectile.Kill();
                    }
                    break;
            }

            Projectile.ai[0] += 1f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, System.Int32 damageDone)
        {
            if (state == 0)
            {
                ExplodeBig();
                AdvanceState();
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (state == 0)
            {
                ExplodeSmall();
                SetState(2);
            }
            return false;
        }
    }
}