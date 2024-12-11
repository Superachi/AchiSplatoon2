using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.Audio;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles
{
    internal class SplatanaChargeProjectile : BaseChargeProjectile
    {
        private float weakSlashShotSpeed;
        private float maxChargeMeleeDamageMod;
        private float maxChargeRangeDamageMod;
        private float maxChargeLifetimeMod;
        private float maxChargeVelocityMod;

        private readonly int soundDelayInterval = 6;
        private SoundStyle chargeSample;

        private int weakSlashProjectile;
        private int strongSlashProjectile;

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseSplatana;

            shootSample = weaponData.ShootSample;
            shootWeakSample = weaponData.ShootWeakSample;
            chargeSample = weaponData.ChargeSample;

            chargeTimeThresholds = weaponData.ChargeTimeThresholds;
            weakSlashShotSpeed = weaponData.WeakSlashShotSpeed;
            maxChargeMeleeDamageMod = weaponData.MaxChargeMeleeDamageMod;
            maxChargeRangeDamageMod = weaponData.MaxChargeRangeDamageMod;
            maxChargeLifetimeMod = weaponData.MaxChargeLifetimeMod;
            maxChargeVelocityMod = weaponData.MaxChargeVelocityMod;

            weakSlashProjectile = weaponData.WeakSlashProjectile;
            strongSlashProjectile = weaponData.StrongSlashProjectile;

            Projectile.damage = weaponData.ActualDamage(Projectile.damage);

            if (WeaponInstance is EelSplatanaWeapon)
            {
                UpdateCurrentColor(ColorHelper.AddRandomHue(30, Color.MediumPurple));
            }
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            ApplyWeaponInstanceData();
            maxChargeTime = chargeTimeThresholds.Last();
            Projectile.velocity = Vector2.Zero;
        }

        protected override void StartCharge()
        {
            // You can do something like playing a sound effect here
        }

        protected override void UpdateCharge(Player owner)
        {
            base.UpdateCharge(owner);

            float rad;
            if (Main.MouseWorld.X > owner.position.X)
            {
                owner.direction = 1;
                rad = MathHelper.ToRadians(-90);
            }
            else
            {
                owner.direction = -1;
                rad = MathHelper.ToRadians(270);
            }

            Vector2 shakeOffset = Main.rand.NextVector2Square(-1, 1) * ChargeQuotient();
            PlayerItemAnimationFaceCursor(owner, offset: shakeOffset, radiansOverride: rad);

            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = soundDelayInterval;

                var pitchValue = -0.5f + (ChargeQuotient() * 0.5f);
                PlayAudio(chargeSample, volume: 0.1f, pitchVariance: 0.1f, maxInstances: 5, pitch: pitchValue);
            }
        }

        protected override void ReleaseCharge(Player owner)
        {
            SoundHelper.StopSoundIfActive(chargeStartAudio);
            hasFired = true;
            var velocity = owner.DirectionTo(Main.MouseWorld) * weakSlashShotSpeed;

            if (chargeLevel > 0)
            {
                PlayAudio(shootSample, pitchVariance: 0.2f);

                var meleeProj = CreateChildProjectile<SplatanaMeleeProjectile>(owner.Center, velocity, MultiplyProjectileDamage(maxChargeMeleeDamageMod));
                meleeProj.wasFullyCharged = true;

                velocity *= maxChargeVelocityMod;
                var proj = CreateChildProjectile(owner.Center, velocity, strongSlashProjectile, MultiplyProjectileDamage(maxChargeRangeDamageMod), triggerSpawnMethods: false)
                    as SplatanaStrongSlashProjectile;
                proj.Projectile.timeLeft = (int)(proj.Projectile.timeLeft * maxChargeLifetimeMod);
                proj.Projectile.penetrate = 3;
                proj.RunSpawnMethods();
            }
            else
            {
                PlayAudio(shootWeakSample, pitchVariance: 0.2f);

                var meleeProj = CreateChildProjectile<SplatanaMeleeProjectile>(owner.Center, velocity, Projectile.damage);
                meleeProj.wasFullyCharged = false;

                CreateChildProjectile(owner.Center, velocity, weakSlashProjectile, Projectile.damage);
            }


            Projectile.Kill();
            return;
        }
    }
}
