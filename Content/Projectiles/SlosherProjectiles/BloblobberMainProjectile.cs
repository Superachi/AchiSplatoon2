using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SlosherProjectiles
{
    internal class BloblobberMainProjectile : BaseProjectile
    {
        public int burstNPCTarget = -1;
        public int burstHitCount;
        public int burstRequiredHits;
        private int burstShotDelay;

        private string shootSampleAlt;
        private int weaponDamage;
        private int shotsLeft;

        public override void SetDefaults()
        {
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as Bloblobber;

            // The slosher child projectiles should do the damage here
            weaponDamage = Projectile.damage;
            shootSample = weaponData.ShootSample;
            shootSampleAlt = weaponData.ShootWeakSample;

            burstRequiredHits = weaponData.BurstShotCount;
            shotsLeft = weaponData.BurstShotCount;
            burstShotDelay = weaponData.BurstShotDelay;
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
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
            var player = GetOwner();

            Timer++;
            if (Timer % burstShotDelay == 0 && shotsLeft > 0)
            {
                Timer = 0;

                shotsLeft--;

                if (IsThisClientTheProjectileOwner())
                {
                    CreateChildProjectile(
                        position: player.Center,
                        velocity: player.DirectionTo(Main.MouseWorld) * 1f,
                        type: ModContent.ProjectileType<BloblobberChildProjectile>(),
                        damage: weaponDamage);
                }
            }
        }
    }
}
