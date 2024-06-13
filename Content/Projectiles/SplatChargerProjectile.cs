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

namespace AchiSplatoon2.Content.Projectiles
{
    internal class SplatChargerProjectile : BaseChargeProjectile
    {
        private const int timeLeftAfterFiring = 120;

        protected override float[] ChargeTimeThresholds { get => [55f]; }
        protected override string ShootSample { get => "SplatChargerShoot"; }
        protected override string ShootWeakSample { get => "SplatChargerShootWeak"; }
        protected virtual bool ShakeScreenOnChargeShot { get => true; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.extraUpdates = 32;
            Projectile.penetrate = 10;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Initialize();
            Projectile.velocity = Vector2.Zero;
            PlayAudio("ChargeStart", volume: 0.2f, pitchVariance: 0.1f, maxInstances: 1);
        }

        protected override void ReleaseCharge(Player owner)
        {
            // Release the attack
            hasFired = true;
            Projectile.friendly = true;

            // Adjust behaviour depending on the charge amount
            if (chargeLevel > 0)
            {
                Projectile.timeLeft = timeLeftAfterFiring * Projectile.extraUpdates;
                PlayAudio(ShootSample, volume: 0.4f, maxInstances: 1);

                if (ShakeScreenOnChargeShot)
                {
                    PunchCameraModifier modifier = new PunchCameraModifier(
                        startPosition: owner.Center,
                        direction: (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(),
                        strength: 4f,
                        vibrationCyclesPerSecond: 8f,
                        frames: 10, 80f, FullName);
                    Main.instance.CameraModifiers.Add(modifier);
                }
            }
            else
            {
                Projectile.penetrate = 1;
                int chargeTimeNormalized = Convert.ToInt32(ChargeTime / Projectile.extraUpdates);

                // Deal a min. of 10% damage and a max. of 40% damage
                Projectile.damage = Convert.ToInt32(
                    (Projectile.damage * 0.1) + ((Projectile.damage * 0.3) * chargeTimeNormalized / ChargeTimeThresholds[0])
                );

                // Similar for the range (min. 3% range, max. 20%)
                Projectile.timeLeft = Convert.ToInt32(
                    (timeLeftAfterFiring * 0.03) + ((timeLeftAfterFiring * 0.17) * chargeTimeNormalized / ChargeTimeThresholds[0])
                ) * Projectile.extraUpdates;

                PlayAudio(ShootWeakSample, volume: 0.4f, maxInstances: 1);
            }

            Projectile.velocity = owner.DirectionTo(Main.MouseWorld) * 3f;
            SyncProjectilePosWithWeaponBarrel(Projectile.position, Projectile.velocity, new SplatCharger());
            PlayAudio("ChargeStart", volume: 0.0f, maxInstances: 1);
            return;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (IsThisClientTheProjectileOwner())
            {
                if (owner.channel && !hasFired)
                {
                    UpdateCharge(owner);
                    return;
                }

                if (!hasFired)
                {
                    ReleaseCharge(owner);
                    return;
                }
            }

            Color dustColor = GenerateInkColor();
            var randomDustVelocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: randomDustVelocity, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.6f));
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Projectile.velocity / 4, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.6f));
        }

        public override void OnKill(int timeLeft)
        {
            if (chargeLevel == 0) return;
            for (int i = 0; i < 15; i++)
            {
                float random = Main.rand.NextFloat(-5, 5);
                float velX = ((Projectile.velocity.X + random) * -0.5f);
                float velY = ((Projectile.velocity.Y + random) * -0.5f);
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: GenerateInkColor(), Scale: Main.rand.NextFloat(0.8f, 1.6f));
            }
        }
    }
}