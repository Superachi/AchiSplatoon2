using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.IO;
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

        public override void AfterSpawn()
        {
            Initialize();

            BaseStringer weaponData = (BaseStringer)weaponSource;
            chargeTimeThresholds = weaponData.ChargeTimeThresholds;
            shootSample = weaponData.ShootSample;
            shootWeakSample = weaponData.ShootWeakSample;
            shotgunArc = weaponData.ShotgunArc;
            projectileCount = weaponData.ProjectileCount;
            allowStickyProjectiles = weaponData.AllowStickyProjectiles;

            if (IsThisClientTheProjectileOwner())
            {
                PlayAudio("ChargeStart", volume: 0.2f, pitchVariance: 0.1f, maxInstances: 1);
            }
        }

        protected override void ReleaseCharge(Player owner)
        {
            hasFired = true;

            // Prevent division by 0, though we shouldn't end up with this anyway
            if (ChargeTime == 0)
            {
                Projectile.Kill();
                return;
            }

            // Set shot modifiers
            float arcModifier = 2f;
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
                    arcModifier = 1.5f;
                }
                else
                {
                    velocityModifier = 1.5f;
                    arcModifier = 1f;
                }

                if (allowStickyProjectiles) { projectileType = ModContent.ProjectileType<TriStringerProjectile>(); }
            }

            PlayShootSample();

            // The angle that the player aims at (player-to-cursor)
            float aimAngle = MathHelper.ToDegrees(
                owner.DirectionTo(Main.MouseWorld).ToRotation()
            );

            float finalArc = Math.Clamp(shotgunArc * (maxChargeTime / ChargeTime) * arcModifier, shotgunArc, shotgunArc * 5);
            float degreesPerProjectile = finalArc / projectileCount;
            int middleProjectile = projectileCount / 2;
            float degreesOffset = -(middleProjectile * degreesPerProjectile);

            for (int i = 0; i < projectileCount; i++)
            {
                // Convert angle: degrees -> radians -> vector
                float degrees = aimAngle + degreesOffset;
                float radians = MathHelper.ToRadians(degrees);
                Vector2 angleVector = new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
                Vector2 velocity = angleVector * velocityModifier; // * (0.95f + i * 0.05f);

                var muzzleDistance = 50f;
                var spawnPositionOffset = Vector2.Normalize(velocity) * muzzleDistance;

                if (!Collision.CanHitLine(Projectile.position, 0, 0, Projectile.position + spawnPositionOffset, 0, 0))
                {
                    spawnPositionOffset = Vector2.Zero;
                }

                // Spawn projectile
                var proj = CreateChildProjectile(position: Projectile.position + spawnPositionOffset, velocity: velocity, type: projectileType, Projectile.damage, true);

                // Set a number in the arrow
                // Can be used to make them explode in sequence
                proj.Projectile.ai[2] = i;

                // Adjust the angle for the next projectile
                degreesOffset += degreesPerProjectile;
            }

            PlayAudio(soundPath: "ChargeStart", volume: 0f, maxInstances: 1);
            if (NetHelper.IsSinglePlayer())
            {
                Projectile.Kill();
            } else
            {
                NetUpdate(ProjNetUpdateType.ReleaseCharge);
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
            Projectile.timeLeft = 12;
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