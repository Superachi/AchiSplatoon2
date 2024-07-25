using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Items.Weapons.Brellas;

namespace AchiSplatoon2.Content.Projectiles.BrellaProjectiles
{
    internal class BrellaShotgunProjectile : BaseProjectile
    {
        private bool hasFired = false;
        private int projectileCount;
        private float shotgunArc;
        private float shotSpeed;

        public int burstNPCTarget = -1;
        public int burstHitCount = 0;
        public int burstRequiredHits;

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseBrella;
            projectileCount = weaponData.ProjectileCount;
            shotgunArc = weaponData.ShotgunArc;
            shootSample = weaponData.ShootSample;

            burstRequiredHits = projectileCount;
        }

        public override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();

            PlayAudio(shootSample, volume: 0.2f, pitchVariance: 0.1f, maxInstances: 5);
            shotSpeed = Vector2.Distance(Main.LocalPlayer.Center, Main.LocalPlayer.Center + Projectile.velocity);
            Projectile.velocity = Vector2.Zero;
        }

        public override void AI()
        {
            var owner = GetOwner();

            if (!hasFired && IsThisClientTheProjectileOwner())
            {
                hasFired = true;
                SyncProjectilePosWithPlayer(owner);

                float degreesPerProjectile = shotgunArc / projectileCount;
                int middleProjectile = projectileCount / 2;
                float degreesOffset = -(middleProjectile * degreesPerProjectile);

                float aimRad = owner.DirectionTo(Main.MouseWorld).ToRotation();
                float aimDeg = MathHelper.ToDegrees(aimRad);

                for (int i = 0; i < projectileCount; i++)
                {
                    float degreesRand = shotgunArc * Main.rand.NextFloat(-0.2f, 0.2f);
                    float shotSpeedRand = Main.rand.NextFloat(0.8f, 1.2f);

                    float degrees = aimDeg + degreesOffset + degreesRand;
                    float radians = MathHelper.ToRadians(degrees);
                    Vector2 angleVector = radians.ToRotationVector2();
                    Vector2 velocity = angleVector * shotSpeed * shotSpeedRand;

                    var pelletProj = CreateChildProjectile(
                        position: Projectile.position,
                        velocity: velocity,
                        type: ModContent.ProjectileType<BrellaPelletProjectile>(),
                        Projectile.damage);

                    degreesOffset += degreesPerProjectile;
                }

                var meleeProj = CreateChildProjectile<BrellaMeleeProjectile>(
                    position: Projectile.position,
                    velocity: owner.DirectionTo(Main.MouseWorld),
                    Projectile.damage * 3);

                Projectile.timeLeft = 60;
            }
        }
    }
}
