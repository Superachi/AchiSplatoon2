using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Prefixes.BrellaPrefixes;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.BrellaProjectiles
{
    internal class BrellaShotgunProjectile : BaseProjectile
    {
        protected bool hasFired = false;

        protected int meleeProjectileType;
        protected int pelletProjectileType;
        protected int pelletCount;
        protected float shotSpeed;
        protected float shotgunArc;
        protected float shotVelocityRandomRange;

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

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();

            shotSpeed = Vector2.Distance(Main.LocalPlayer.Center, Main.LocalPlayer.Center + Projectile.velocity);
            Projectile.velocity = Vector2.Zero;
            PlayShootSound();
        }

        protected override void ApplyWeaponPrefixData()
        {
            var prefix = PrefixHelper.GetWeaponPrefixById(weaponSourcePrefix);
            prefix?.ApplyProjectileStats(this);

            if (prefix is BaseBrellaPrefix brellaPrefix)
            {
                pelletCount += prefix.ExtraProjectileBonus;
                shotgunArc += prefix.AimVariation;
            }
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

                    Vector2 finalOffset = Vector2.Zero;
                    Vector2 brellaOffset = Vector2.Normalize(velocity) * 40;
                    if (Collision.CanHitLine(Projectile.Center, 1, 1, Projectile.Center + brellaOffset, 1, 1))
                    {
                        finalOffset = brellaOffset;
                    }

                    // Pellet projectile
                    CreateChildProjectile(
                        position: Projectile.position + finalOffset,
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
