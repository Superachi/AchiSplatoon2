using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SlosherProjectiles
{
    internal class SlosherMainProjectile : BaseProjectile
    {
        private string shootSampleAlt;
        private int weaponDamage;
        private float fallDelayCount = 0;
        private float fallSpeed;
        private float terminalVelocity = 8f;
        private bool hasChildren = false;

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

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseSlosher;

            // The slosher child projectiles should do the damage here
            weaponDamage = Projectile.damage;

            shootSample = weaponData.ShootSample;
            shootSampleAlt = weaponData.ShootWeakSample;
            fallSpeed = weaponData.ShotGravity;
        }

        public override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();

            if (Main.rand.NextBool(2))
            {
                PlayAudio(shootSample, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 5);
            }
            else
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

                if (IsThisClientTheProjectileOwner())
                {
                    hasChildren = true;
                    CreateChildProjectile(
                        position: Projectile.Center,
                        velocity: new Vector2(childVelX, childVelY),
                        type: ModContent.ProjectileType<SlosherChildProjectile>(),
                        damage: weaponDamage);
                }
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

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);

            if (hasChildren)
            {
                modifiers.FinalDamage *= 0;
                modifiers.HideCombatText();
                Projectile.Kill();
            }
        }
    }
}
