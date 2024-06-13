using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using rail;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Items.Weapons;
using System.Linq;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class TriStringerCharge : BaseChargeProjectile
    {
        protected override float[] ChargeTimeThresholds { get => [36f, 72f]; }
        protected override string ShootSample { get => "TriStringerShoot"; }
        protected override string ShootWeakSample { get => "BambooChargerShootWeak"; }
        protected virtual float ShotgunArc { get => 4f; }
        protected virtual int ProjectileCount { get => 3; }
        protected virtual bool AllowStickyProjectiles { get => true; }

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
                PlayAudio("BambooChargerShootWeak");
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

                if (AllowStickyProjectiles) { projectileType = ModContent.ProjectileType<TriStringerProjectile>(); }
                PlayAudio("TriStringerShoot");
            }

            // The angle that the player aims at (player-to-cursor)
            float aimAngle = MathHelper.ToDegrees(
                owner.DirectionTo(Main.MouseWorld).ToRotation()
            );

            float finalArc = Math.Clamp(ShotgunArc * (maxChargeTime / ChargeTime) * arcModifier, ShotgunArc, ShotgunArc * 5);
            float degreesPerProjectile = finalArc / ProjectileCount;
            int middleProjectile = ProjectileCount / 2;
            float degreesOffset = -(middleProjectile * degreesPerProjectile);

            for (int i = 0; i < ProjectileCount; i++)
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
                int proj = Projectile.NewProjectile(
                spawnSource: Projectile.GetSource_FromThis(),
                position: Projectile.position + spawnPositionOffset,
                velocity: velocity,
                Type: projectileType,
                Damage: Projectile.damage,
                KnockBack: Projectile.knockBack,
                Owner: Main.myPlayer);

                // Set a number in the arrow
                // Can be used to make them explode in sequence
                Main.projectile[proj].ai[2] = i;

                // Adjust the angle for the next projectile
                degreesOffset += degreesPerProjectile;
            }

            PlayAudio(soundPath: "ChargeStart", volume: 0f, maxInstances: 1);
            Projectile.Kill();
        }
    }
}