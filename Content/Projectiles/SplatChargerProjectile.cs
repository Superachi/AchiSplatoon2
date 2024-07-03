using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class SplatChargerProjectile : BaseChargeProjectile
    {
        private const int timeLeftAfterFiring = 120;
        private bool firstHit = false;

        protected virtual bool ShakeScreenOnChargeShot { get => true; }
        protected virtual int MaxPenetrate { get => 10; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.extraUpdates = 32;
        }

        public override void AfterSpawn()
        {
            Initialize();
            Projectile.velocity = Vector2.Zero;

            if (IsThisClientTheProjectileOwner())
            {
                PlayAudio("ChargeStart", volume: 0.2f, pitchVariance: 0.1f, maxInstances: 1);
            }

            BaseCharger weaponData = (BaseCharger)weaponSource;

            chargeTimeThresholds = weaponData.ChargeTimeThresholds;
            shootSample = weaponData.ShootSample;
            shootWeakSample = weaponData.ShootWeakSample;
        }

        protected override void ReleaseCharge(Player owner)
        {
            // Release the attack
            hasFired = true;
            Projectile.friendly = true;
            Projectile.tileCollide = true;

            // Adjust behaviour depending on the charge amount
            if (chargeLevel > 0)
            {
                Projectile.penetrate = MaxPenetrate + piercingModifier;
                Projectile.timeLeft = timeLeftAfterFiring * Projectile.extraUpdates;

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
                Projectile.penetrate = 1 + piercingModifier;
                int chargeTimeNormalized = Convert.ToInt32(ChargeTime / Projectile.extraUpdates);

                // Deal a min. of 10% damage and a max. of 40% damage
                Projectile.damage = Convert.ToInt32(
                    (Projectile.damage * 0.1) + ((Projectile.damage * 0.3) * chargeTimeNormalized / chargeTimeThresholds[0])
                );

                // Similar for the range (min. 3% range, max. 20%)
                Projectile.timeLeft = Convert.ToInt32(
                    (timeLeftAfterFiring * 0.03) + ((timeLeftAfterFiring * 0.17) * chargeTimeNormalized / chargeTimeThresholds[0])
                ) * Projectile.extraUpdates;
            }

            PlayShootSample();

            Projectile.velocity = owner.DirectionTo(Main.MouseWorld) * 3f;
            SyncProjectilePosWithWeaponBarrel(Projectile.position, Projectile.velocity, new SplatCharger());
            PlayAudio("ChargeStart", volume: 0.0f, maxInstances: 1);
            NetUpdate(ProjNetUpdateType.ReleaseCharge);
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

            if (hasFired)
            {
                DustTrail();
            }
        }

        private void DustTrail()
        {
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!firstHit && IsChargeMaxedOut())
            {
                firstHit = true;
                DirectHitDustBurst(target.Center);
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
            writer.WriteVector2(Projectile.velocity);
            writer.WriteVector2(Projectile.position);
            writer.Write((byte)chargeLevel);
            writer.Write((string)shootSample);
            writer.Write((string)shootWeakSample);
        }

        protected override void NetReceiveReleaseCharge(BinaryReader reader)
        {
            Projectile.velocity = reader.ReadVector2();
            Projectile.position = reader.ReadVector2();
            chargeLevel = reader.ReadByte();
            shootSample = reader.ReadString();
            shootWeakSample = reader.ReadString();

            PlayShootSample();
            hasFired = true;
        }
    }
}