using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AchiSplatoon2.Helpers;
using System.IO;

namespace AchiSplatoon2.Content.Projectiles.SlosherProjectiles
{
    internal class SlosherChildProjectile : BaseProjectile
    {
        private float delayUntilFall = 3f;
        private float fallSpeed;
        private float terminalVelocity = 12f;

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 2;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Initialize();

            BaseSlosher weaponData = (BaseSlosher)weaponSource;
            fallSpeed = weaponData.ShotGravity;
        }

        public override void AI()
        {
            if (isFakeDestroyed) return;
            Projectile.ai[0] += 1f;

            // Start falling eventually
            if (Projectile.ai[0] >= delayUntilFall * FrameSpeed())
            {
                Projectile.velocity.Y += fallSpeed;

                if (Projectile.velocity.Y >= 0)
                {
                    Projectile.velocity.X *= 0.99f;
                }
            }

            if (Projectile.velocity.Y > terminalVelocity)
            {
                Projectile.velocity.Y = terminalVelocity;
            }

            Color dustColor = GenerateInkColor();
            Dust.NewDustPerfect(Position: Projectile.Center, Type: ModContent.DustType<SlosherProjectileDust>(), Velocity: Projectile.velocity / 4, newColor: dustColor, Scale: Main.rand.NextFloat(1.4f, 2f));
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (isFakeDestroyed) return false;
            for (int i = 0; i < 5; i++)
            {
                float random = Main.rand.NextFloat(-2, 2);
                float velX = ((Projectile.velocity.X + random) * -0.5f);
                float velY = ((Projectile.velocity.Y + random) * -0.5f);
                int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: GenerateInkColor(), Scale: Main.rand.NextFloat(0.8f, 1.6f));
            }

            if (IsThisClientTheProjectileOwner() && !NetHelper.IsSinglePlayer())
            {
                FakeDestroy();
                return false;
            }
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 18;
            base.OnHitNPC(target, hit, damageDone);
        }
    }
}
