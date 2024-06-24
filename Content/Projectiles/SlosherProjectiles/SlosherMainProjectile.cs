using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using Microsoft.Xna.Framework;
using Mono.Cecil;

namespace AchiSplatoon2.Content.Projectiles.SlosherProjectiles
{
    internal class SlosherMainProjectile : BaseProjectile
    {
        private string shootSampleAlt;
        private int weaponDamage;
        private float fallDelayCount = 0;
        private float fallSpeed;
        private float terminalVelocity = 8f;

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 2;
            Projectile.width = 8;
            Projectile.height = 8;
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

            // The slosher child projectiles should do the damage here
            weaponDamage = Projectile.damage;

            shootSample = weaponData.ShootSample;
            shootSampleAlt = weaponData.ShootWeakSample;
            fallSpeed = weaponData.ShotGravity;

            if (Main.rand.NextBool(2))
            {
                PlayAudio(shootSample, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 5);
            } else
            {
                PlayAudio(shootSampleAlt, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 5);
            }
        }

        protected float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            Timer++;

            if (Timer % 5 == 0)
            {
                Timer = 0;
                fallDelayCount++;
                float childVelX = Projectile.velocity.X * 0.5f;
                float childVelY = Projectile.velocity.Y * 0.25f;

                Projectile.NewProjectile(
                spawnSource: Projectile.GetSource_FromThis(),
                position: Projectile.Center,
                velocity: new Vector2(childVelX, childVelY),
                Type: ModContent.ProjectileType<SlosherChildProjectile>(),
                Damage: weaponDamage,
                KnockBack: Projectile.knockBack,
                Owner: Main.myPlayer);
            }

            // Start falling eventually
            if (fallDelayCount > 2)
            {
                Projectile.velocity.Y += fallSpeed;
            }

            if (Projectile.velocity.Y >= 0)
            {
                Projectile.velocity.X *= 0.995f;
            }

            if (Projectile.velocity.Y > terminalVelocity)
            {
                Projectile.velocity.Y = terminalVelocity;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 18;
            base.OnHitNPC(target, hit, damageDone);
        }
    }
}
