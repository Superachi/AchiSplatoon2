using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
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
        private SlotId chargeLoopSound;

        private int weakSlashProjectile;
        private int strongSlashProjectile;
        private int meleeEnergyProjectile;

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
            meleeEnergyProjectile = weaponData.MeleeEnergyProjectile;

            if (!weaponData.EnableWeakSlashProjectile) weakSlashProjectile = -1;
            if (!weaponData.EnableStrongSlashProjectile) strongSlashProjectile = -1;

            Projectile.damage = weaponData.ActualDamage(Projectile.damage);

            if (WeaponInstance is EelSplatana)
            {
                colorOverride = ColorHelper.AddRandomHue(30, Color.MediumPurple);
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
                chargeLoopSound = PlayAudio(chargeSample, volume: 0.05f, pitchVariance: 0.05f, maxInstances: 5, pitch: pitchValue);
            }
        }

        protected override void ReleaseCharge(Player owner)
        {
            SoundHelper.StopSoundIfActive(chargeStartAudio);
            SoundHelper.StopSoundIfActive(chargeLoopSound);
            hasFired = true;
            var velocity = owner.DirectionTo(Main.MouseWorld) * weakSlashShotSpeed;
            var color = colorOverride ?? GenerateInkColor();

            if (chargeLevel > 0)
            {
                PlayAudio(shootSample, pitchVariance: 0.2f);

                var meleeProj = CreateChildProjectile<SplatanaMeleeProjectile>(owner.Center, velocity, 0);
                meleeProj.wasFullyCharged = true;
                meleeProj.UpdateCurrentColor(color);

                if (meleeEnergyProjectile != -1)
                {
                    var proj = CreateChildProjectile(owner.Center, Vector2.Zero, meleeEnergyProjectile, MultiplyProjectileDamage(maxChargeMeleeDamageMod), triggerSpawnMethods: false);
                    proj.UpdateCurrentColor(color);
                    proj.RunSpawnMethods();
                }

                if (strongSlashProjectile != -1)
                {
                    velocity *= maxChargeVelocityMod;
                    var proj = CreateChildProjectile(owner.Center, velocity, strongSlashProjectile, MultiplyProjectileDamage(maxChargeRangeDamageMod), triggerSpawnMethods: false)
                        as SplatanaStrongSlashProjectile;
                    proj.RunSpawnMethods();
                    proj.UpdateCurrentColor(color);
                    proj.Projectile.timeLeft = (int)(proj.Projectile.timeLeft * maxChargeLifetimeMod);
                    proj.Projectile.penetrate += 2;
                }
            }
            else
            {
                PlayAudio(shootWeakSample, pitchVariance: 0.2f);

                var meleeProj = CreateChildProjectile<SplatanaMeleeProjectile>(owner.Center, velocity, Projectile.damage);
                meleeProj.wasFullyCharged = false;
                meleeProj.UpdateCurrentColor(color);

                if (weakSlashProjectile != -1)
                {
                    var proj = CreateChildProjectile(owner.Center, velocity, weakSlashProjectile, Projectile.damage);
                    proj.UpdateCurrentColor(color);
                }
            }

            Projectile.Kill();
            return;
        }
    }
}
