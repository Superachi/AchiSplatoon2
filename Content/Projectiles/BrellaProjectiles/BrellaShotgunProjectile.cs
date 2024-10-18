using AchiSplatoon2.Content.Items.Weapons.Brellas;
using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.BrellaProjectiles
{
    internal class BrellaShotgunProjectile : BaseProjectile
    {
        private bool hasFired = false;

        private int meleeProjectileType;
        private int pelletProjectileType;
        private int pelletCount;
        protected float shotSpeed;
        private float shotgunArc;
        private float shotVelocityRandomRange;

        public int burstNPCTarget = -1;
        public int burstHitCount = 0;
        public int burstRequiredHits;

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = (BaseBrella)WeaponInstance;

            meleeProjectileType = weaponData.MeleeProjectileType;
            pelletProjectileType = weaponData.ProjectileType;
            pelletCount = weaponData.ProjectileCount;
            shotgunArc = weaponData.ShotgunArc;
            shootSample = weaponData.ShootSample;
            shotVelocityRandomRange = weaponData.ShotVelocityRandomRange;

            burstRequiredHits = pelletCount;
        }

        public override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();

            shotSpeed = Vector2.Distance(Main.LocalPlayer.Center, Main.LocalPlayer.Center + Projectile.velocity);
            Projectile.velocity = Vector2.Zero;
            PlayShootSound();
        }

        protected override void PlayShootSound()
        {
            PlayAudio(shootSample, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 5);
        }

        public override void AI()
        {
            var owner = GetOwner();

            if (!hasFired && IsThisClientTheProjectileOwner())
            {
                hasFired = true;
                SyncProjectilePosWithPlayer(owner);

                float degreesPerProjectile = shotgunArc / pelletCount;
                int middleProjectile = pelletCount / 2;
                float degreesOffset = -(middleProjectile * degreesPerProjectile);

                float aimRad = owner.DirectionTo(Main.MouseWorld).ToRotation();
                float aimDeg = MathHelper.ToDegrees(aimRad);

                for (int i = 0; i < pelletCount; i++)
                {
                    float degreesRand = shotgunArc * Main.rand.NextFloat(-0.2f, 0.2f);
                    float shotSpeedRand = Main.rand.NextFloat(1 - shotVelocityRandomRange, 1 + shotVelocityRandomRange);

                    float degrees = aimDeg + degreesOffset + degreesRand;
                    float radians = MathHelper.ToRadians(degrees);
                    Vector2 angleVector = radians.ToRotationVector2();
                    Vector2 velocity = angleVector * shotSpeed * shotSpeedRand;

                    // Pellet projectile
                    CreateChildProjectile(
                        position: Projectile.position,
                        velocity: velocity,
                        type: pelletProjectileType,
                        Projectile.damage);

                    degreesOffset += degreesPerProjectile;
                }

                // Melee projectile
                CreateChildProjectile(
                    position: Projectile.position,
                    velocity: owner.DirectionTo(Main.MouseWorld),
                    type: meleeProjectileType,
                    Projectile.damage);

                Projectile.timeLeft = 60;
            }
        }
    }
}
