using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class TriStringerCharge : BaseChargeProjectile
    {
        protected float shotgunArc;
        protected int projectileCount;
        protected bool allowStickyProjectiles;
        public int burstHitCount = 0;
        public int burstNPCTarget = -1;
        public int burstRequiredHits;
        private float muzzleDistance;

        public override void AfterSpawn()
        {
            Initialize();

            BaseStringer weaponData = (BaseStringer)weaponSource;
            muzzleDistance = weaponData.MuzzleOffsetPx;
            chargeTimeThresholds = weaponData.ChargeTimeThresholds;
            shootSample = weaponData.ShootSample;
            shootWeakSample = weaponData.ShootWeakSample;
            shotgunArc = weaponData.ShotgunArc;
            projectileCount = weaponData.ProjectileCount;

            allowStickyProjectiles = weaponData.AllowStickyProjectiles;
            burstRequiredHits = weaponData.ProjectileCount;

            if (IsThisClientTheProjectileOwner())
            {
                PlayAudio("ChargeStart", volume: 0.2f, pitchVariance: 0.1f, maxInstances: 1);
            }
        }

        protected override void ReleaseCharge(Player owner)
        {
            if (isFakeDestroyed) return;
            hasFired = true;

            // Prevent division by 0, though we shouldn't end up with this anyway
            if (ChargeTime == 0)
            {
                Projectile.Kill();
                return;
            }

            // Set shot modifiers
            float velocityModifier = 0.75f;
            int projectileType = ModContent.ProjectileType<TriStringerProjectileWeak>();
            if (chargeLevel == 0)
            {
                Projectile.damage /= 2;
            }
            else
            {
                if (chargeLevel == 1)
                {
                    Projectile.damage = (int)(Projectile.damage * 0.75);
                    velocityModifier = 1f;
                }
                else
                {
                    velocityModifier = 1.5f;
                }

                if (allowStickyProjectiles) { projectileType = ModContent.ProjectileType<TriStringerProjectile>(); }
            }

            PlayShootSample();

            float chargePercentage = Math.Clamp(ChargeTime / chargeTimeThresholds.Last(), 0.2f , 1f);
            float finalArc = shotgunArc / chargePercentage / (projectileCount / 2);

            var mP = owner.GetModPlayer<InkWeaponPlayer>();
            if (mP.hasFreshQuiver && IsChargeMaxedOut())
            {
                finalArc *= mP.freshQuiverArcMod;
                velocityModifier *= mP.freshQuiverVelocityMod;
            }

            float degreesPerProjectile = finalArc / projectileCount;
            int middleProjectile = projectileCount / 2;
            float degreesOffset = -(middleProjectile * degreesPerProjectile);

            // Convert angle: degrees -> radians -> vector
            float aimAngle = MathHelper.ToDegrees(
                owner.DirectionTo(Main.MouseWorld).ToRotation()
            );

            for (int i = 0; i < projectileCount; i++)
            {
                float degrees = aimAngle + degreesOffset;
                float radians = MathHelper.ToRadians(degrees);
                Vector2 angleVector = radians.ToRotationVector2();
                Vector2 velocity = angleVector * velocityModifier;

                var spawnPositionOffset = Vector2.Normalize(velocity) * muzzleDistance;
                if (!Collision.CanHitLine(Projectile.position, 0, 0, Projectile.position + spawnPositionOffset, 0, 0))
                {
                    spawnPositionOffset = Vector2.Zero;
                }

                // Spawn projectile
                var p = CreateChildProjectile(position: Projectile.position + spawnPositionOffset, velocity: velocity, type: projectileType, Projectile.damage, false);
                var modProj = p as TriStringerProjectile;
                modProj.parentFullyCharged = IsChargeMaxedOut();
                modProj.firedWithFreshQuiver = mP.hasFreshQuiver;

                // Set a number in the arrow
                // Can be used to make them explode in sequence
                p.Projectile.ai[2] = i;
                p.AfterSpawn();

                // Adjust the angle for the next projectile
                degreesOffset += degreesPerProjectile;
            }

            PlayAudio(soundPath: "ChargeStart", volume: 0f, maxInstances: 1);
            Projectile.timeLeft = 60;
            FakeDestroy();
            NetUpdate(ProjNetUpdateType.ReleaseCharge);
        }

        public override void AI()
        {
            if (!isFakeDestroyed)
            {
                base.AI();
            }
        }

        private void PlayShootSample()
        {
            if (chargeLevel > 0)
            {
                PlayAudio(shootSample, volume: 0.4f, maxInstances: 1);
            }
            else
            {
                PlayAudio(shootWeakSample, volume: 0.4f, maxInstances: 1);
            }
        }

        protected override void NetSendReleaseCharge(BinaryWriter writer)
        {
            writer.Write((byte)chargeLevel);
            writer.Write((string)shootSample);
            writer.Write((string)shootWeakSample);
        }

        protected override void NetReceiveReleaseCharge(BinaryReader reader)
        {
            chargeLevel = reader.ReadByte();
            shootSample = reader.ReadString();
            shootWeakSample = reader.ReadString();

            PlayShootSample();
            hasFired = true;
        }
    }
}