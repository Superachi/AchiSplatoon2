using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SplatlingProjectiles.Charges
{
    internal class HeavySplatlingCharge : BaseChargeProjectile
    {
        protected virtual int ProjectileType { get => ModContent.ProjectileType<HeavySplatlingProjectile>(); }
        private float muzzleDistance;
        private float barrageMaxAmmo;
        private float barrageVelocity;
        private float barrageShotTime;
        private float damageChargeMod = 1f;
        private float velocityChargeMod = 1f;
        private int soundDelayInterval = 5;

        protected float ChargedAmmo
        {
            get => Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        public override void AfterSpawn()
        {
            Initialize();

            BaseSplatling weaponData = (BaseSplatling)weaponSource;
            muzzleDistance = weaponData.MuzzleOffsetPx;
            chargeTimeThresholds = weaponData.ChargeTimeThresholds;
            barrageMaxAmmo = weaponData.BarrageMaxAmmo;
            barrageVelocity = weaponData.BarrageVelocity;
            barrageShotTime = weaponData.BarrageShotTime;

            Projectile.soundDelay = 30;
        }

        protected override void StartCharge()
        {
            PlayAudio(soundPath: "SplatlingChargeStart", volume: 0.2f, pitchVariance: 0.1f, maxInstances: 1);
        }

        protected override void ReleaseCharge(Player owner)
        {
            hasFired = true;
            ChargedAmmo = Convert.ToInt32(barrageMaxAmmo * (ChargeTime / MaxChargeTime()));
            ChargeTime = 0;
            StopAudio(soundPath: "ChargeStart");
            StopAudio(soundPath: "SplatlingChargeStart");
            StopAudio(soundPath: "SplatlingChargeLoop");

            // Set the damage modifier
            switch (chargeLevel)
            {
                case 0:
                    damageChargeMod = 0.6f;
                    velocityChargeMod = 0.6f;
                    barrageShotTime++;
                    break;
                case 1:
                    damageChargeMod = 0.8f;
                    velocityChargeMod = 0.8f;
                    break;
                case 2:
                    damageChargeMod = 1f;
                    velocityChargeMod = 1f;
                    break;
            }
        }

        protected override void UpdateCharge(Player owner)
        {
            base.UpdateCharge(owner);

            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = (int)(soundDelayInterval * (MaxChargeTime() / ChargeTime));
                var pitchValue = 0.6f + (ChargeTime / MaxChargeTime()) * 0.5f;

                PlayAudio(soundPath: "SplatlingChargeLoop", volume: 0.1f, pitchVariance: 0.1f, maxInstances: 5, pitch: pitchValue);
            }
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (IsThisClientTheProjectileOwner())
            {
                if (owner.dead) { Projectile.Kill(); return; }
                if (owner.channel)
                {
                    UpdateCharge(owner);
                    return;
                }

                if (!hasFired)
                {
                    ReleaseCharge(owner);
                    return;
                }

                if (ChargedAmmo > 0)
                {
                    SyncProjectilePosWithPlayer(owner, 0, 8);

                    ChargeTime++;
                    if (ChargeTime >= barrageShotTime)
                    {
                        // If ChargeTime were directly set to 0, it would not work nicely with non-decimal values (eg. when attack speed is increased)
                        ChargeTime -= barrageShotTime;
                        ChargedAmmo--;
                        var recoilVector = owner.DirectionTo(Main.MouseWorld) * -3;
                        PlayerItemAnimationFaceCursor(owner, recoilVector);

                        // Calculate angle/velocity
                        lastShotRadians = owner.DirectionTo(Main.MouseWorld).ToRotation();
                        float aimAngle = MathHelper.ToDegrees(lastShotRadians);

                        float radians = MathHelper.ToRadians(aimAngle);
                        Vector2 angleVector = radians.ToRotationVector2();
                        Vector2 velocity = angleVector * barrageVelocity * velocityChargeMod;

                        // Spawn the projectile
                        var spawnPositionOffset = Vector2.Normalize(velocity) * muzzleDistance;

                        if (!Collision.CanHit(Projectile.position, 0, 0, Projectile.position + spawnPositionOffset, 0, 0))
                        {
                            spawnPositionOffset = Vector2.Zero;
                        }

                        var p = CreateChildProjectile(
                            position: Projectile.position + spawnPositionOffset,
                            velocity: velocity,
                            type: ProjectileType,
                            damage: Convert.ToInt32(Projectile.damage * damageChargeMod),
                            triggerAfterSpawn: false);

                        var spreadOffset = 0.5f;
                        p.Projectile.velocity.X += Main.rand.NextFloat(-spreadOffset, spreadOffset);
                        p.Projectile.velocity.Y += Main.rand.NextFloat(-spreadOffset, spreadOffset);
                        p.AfterSpawn();
                        DebugHelper.PrintInfo($"generating: {p.Projectile.velocity}");

                        for (int i = 0; i < 15; i++)
                        {
                            Color dustColor = GenerateInkColor();

                            float random = Main.rand.NextFloat(-5, 5);
                            float velX = ((velocity.X + random) * 0.5f);
                            float velY = ((velocity.Y + random) * 0.5f);

                            Dust.NewDust(Projectile.position + spawnPositionOffset, 1, 1, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
                        }

                        // Sync shoot animation
                        // Set item use animation and angle
                        NetUpdate(ProjNetUpdateType.ShootAnimation);
                    }

                    return;
                }

                Projectile.Kill();
            }
        }

        protected override void NetSendShootAnimation(BinaryWriter writer)
        {
            Player owner = Main.player[Projectile.owner];

            writer.Write((double)lastShotRadians);
            writer.Write((Int16)owner.itemAnimationMax);
        }

        protected override void NetReceiveShootAnimation(BinaryReader reader)
        {
            Player owner = Main.player[Projectile.owner];

            // Make weapon face client's cursor
            lastShotRadians = (float)reader.ReadDouble();
            Vector2 rotationVector = lastShotRadians.ToRotationVector2();
            PlayerItemAnimationFaceCursor(owner, rotationVector * -3, lastShotRadians);

            // Set the animation time
            owner.itemAnimation = reader.ReadInt16();
            owner.itemTime = owner.itemAnimation;
        }
    }
}
