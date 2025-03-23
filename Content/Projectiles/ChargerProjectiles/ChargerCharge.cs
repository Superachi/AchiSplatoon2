using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.ProjectileVisuals;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System;
using Terraria;

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

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            ApplyWeaponInstanceData();

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

            var c = CreateChildProjectile(position: position, velocity: velocity, type: ProjectileType, Projectile.damage, false);
            var proj = c.Projectile;
            var modProj = (ChargerProjectile)c.Projectile.ModProjectile;

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
                    GameFeelHelper.ShakeScreenNearPlayer(owner, true);
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