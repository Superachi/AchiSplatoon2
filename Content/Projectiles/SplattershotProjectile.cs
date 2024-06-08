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
        private float delayUntilFall = 14f;
        private float fallSpeed = 0.6f;
        private float terminalVelocity = 8f;
        
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
        {
            var sample = new SoundStyle("AchiSplatoon2/Content/Items/SplattershotShoot");
            var shootSound = sample with
            {
                Volume = 0.3f,
                PitchVariance = 0.1f,
                MaxInstances = 3
            };
            SoundEngine.PlaySound(shootSound);
            //SoundEngine.PlaySound(SoundID.Item39, Projectile.position);
            //SoundEngine.PlaySound(SoundID.Item118, Projectile.position);
            Projectile.velocity.X += Main.rand.Next(-1, 1);
            Projectile.velocity.Y += Main.rand.Next(-1, 1);
        }

        public override void AI()
        {
            // Face direction
            Projectile.rotation = Projectile.velocity.ToRotation();

            // Start falling eventually
            if (Projectile.ai[0] >= delayUntilFall)
            {
                Projectile.velocity.Y = Projectile.velocity.Y + fallSpeed;
            } else
            {
                Projectile.ai[0] += 1f;
            }
            if (Projectile.velocity.Y > terminalVelocity)
            {
                Projectile.velocity.Y = terminalVelocity;
            }

            // Wind resistance
            Projectile.velocity.X = Projectile.velocity.X * 0.99f;
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
    }
}