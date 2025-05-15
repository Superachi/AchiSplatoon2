using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Prefixes.ChargerPrefixes;
using AchiSplatoon2.Content.Prefixes.ChargeWeaponPrefixes;
using AchiSplatoon2.Content.Projectiles.ProjectileVisuals;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.ChargerProjectiles
{
    internal class ChargerCharge : BaseChargeProjectile
    {
        private const int baseShotLifeTimeAfterFiring = 20;

        protected int ProjectileType { get; private set; }

        protected virtual bool ShakeScreenOnChargeShot { get; private set; }
        protected virtual int MaxPenetrate { get; private set; }
        protected float RangeModifier { get; private set; }
        protected float MinPartialRange { get; private set; }
        protected float MaxPartialRange { get; private set; }
        protected bool DirectHitEffect { get; private set; }

        private SlotId chargeStartAudio;
        private int _projectileCount;

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = (BaseCharger)WeaponInstance;

            ProjectileType = weaponData.ProjectileType;

            chargeTimeThresholds = weaponData.ChargeTimeThresholds;
            ShakeScreenOnChargeShot = weaponData.ScreenShake;
            MaxPenetrate = weaponData.MaxPenetrate;

            RangeModifier = weaponData.RangeModifier;
            MinPartialRange = weaponData.MinPartialRange;
            MaxPartialRange = weaponData.MaxPartialRange;
            DirectHitEffect = weaponData.DirectHitEffect;
        }

        protected override void ApplyWeaponPrefixData()
        {
            base.ApplyWeaponPrefixData();
            var prefix = PrefixHelper.GetWeaponPrefixById(weaponSourcePrefix);

            _projectileCount = 1;

            if (prefix is TwinPrefix)
            {
                _projectileCount = 2;
            }
            else if (prefix is ExplosivePrefix)
            {
                MaxPenetrate = 1;
            }
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            ApplyWeaponInstanceData();
            DestroyOtherOwnedChargeProjectiles();

            Projectile.velocity = Vector2.Zero;

            if (IsThisClientTheProjectileOwner())
            {
                chargeStartAudio = PlayAudio(SoundPaths.ChargeStart.ToSoundStyle(), volume: 0.2f, pitchVariance: 0.1f, maxInstances: 1);
            }
        }

        public override void AI()
        {
            base.AI();
            if (IsThisClientTheProjectileOwner() && IsChargeMaxedOut())
            {
                SoundHelper.StopSoundIfActive(chargeStartAudio);
            }
        }

        private void FireProjectile(Vector2 position, Vector2 velocity)
        {
            float degreesPerProjectile = TwinPrefix.TwinShotArc;
            int middleProjectile = _projectileCount / 2;
            float degreesOffset = -(middleProjectile * degreesPerProjectile);

            if (_projectileCount % 2 == 0)
            {
                degreesOffset += degreesPerProjectile / 2;
            }

            // Convert angle: degrees -> radians -> vector
            float aimAngle = MathHelper.ToDegrees(
                Owner.DirectionTo(Main.MouseWorld).ToRotation()
            );

            for (int i = 0; i < _projectileCount; i++)
            {
                float degrees = aimAngle + degreesOffset;
                float radians = MathHelper.ToRadians(degrees);
                Vector2 angleVector = radians.ToRotationVector2();
                Vector2 newVelocity = angleVector * velocityModifier;

                // Spawn projectile
                var childProj = CreateChildProjectile(position: position, velocity: newVelocity, type: ProjectileType, Projectile.damage, false);
                var proj = childProj.Projectile;
                var modProj = (ChargerProjectile)childProj.Projectile.ModProjectile;

                SetProjectileProperties(proj, modProj);

                // Adjust the angle for the next projectile
                degreesOffset += degreesPerProjectile;
            }
        }

        private void SetProjectileProperties(Projectile proj, ChargerProjectile modProj)
        {
            if (IsChargeMaxedOut())
            {
                // Range
                proj.timeLeft = baseShotLifeTimeAfterFiring * proj.extraUpdates;

                // Pierce
                modProj.penetrateOverride = MaxPenetrate + piercingModifier;
                if (proj.penetrate != 1) wormDamageReduction = true;

                // Visual
                modProj.enableDirectHitEffect = DirectHitEffect;
                if (ShakeScreenOnChargeShot)
                {
                    GameFeelHelper.ShakeScreenNearPlayer(Owner, true);
                }
            }
            else
            {
                // Range + damage
                var finalQuotient = Math.Clamp(ChargeQuotient(), MinPartialRange, MaxPartialRange);
                proj.damage = (int)(proj.damage * finalQuotient) / 3;
                proj.timeLeft = (int)(baseShotLifeTimeAfterFiring * proj.extraUpdates * finalQuotient);

                // Pierce
                modProj.penetrateOverride = 1 + piercingModifier;
                if (proj.penetrate != 1) wormDamageReduction = true;

                // Visual
                modProj.enableDirectHitEffect = false;
            }

            modProj.wasParentChargeMaxed = IsChargeMaxedOut();
            proj.timeLeft = (int)(proj.timeLeft * RangeModifier);
            modProj.RunSpawnMethods();
            modProj.UpdateCurrentColor(Owner.GetModPlayer<ColorChipPlayer>().GetColorFromChips());
        }

        protected override void ReleaseCharge(Player owner)
        {
            // Release the attack
            hasFired = true;

            // Offset the projectile's position to match the weapon
            var velocity = owner.DirectionTo(Main.MouseWorld);
            Vector2 weaponOffset = new Vector2(62, 2);

            Vector2 position = GetOwner().Center;
            Vector2 desiredPosition = position + WoomyMathHelper.AddRotationToVector2(weaponOffset, MathHelper.ToDegrees(velocity.ToRotation()));
            bool canHit = Collision.CanHit(position, 0, 0, desiredPosition, 0, 0);
            if (canHit)
            {
                position = desiredPosition;
            }

            FireProjectile(position, velocity);

            SoundHelper.StopSoundIfActive(chargeStartAudio);
            Projectile.Kill();
        }

        protected override void ChargeLevelUpEffect()
        {
            base.ChargeLevelUpEffect();
            if (IsChargeMaxedOut())
            {
                CreateChildProjectile<WeaponChargeSparkleVisual>(Owner.Center, Vector2.Zero, 0, true);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!IsThisClientTheProjectileOwner() && !GetOwner().channel) return false;

            DrawStraightTrajectoryLine();
            return false;
        }
    }
}