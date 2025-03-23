using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.ProjectileVisuals;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SplatlingProjectiles.Charges
{
    internal class SnipewriterCharge : BaseChargeProjectile
    {
        protected virtual int ProjectileType { get => ModContent.ProjectileType<SnipewriterProjectile>(); }
        protected float muzzleDistance;
        protected float barrageMaxAmmo;
        protected float barrageVelocity;
        protected float barrageShotTime;
        protected float damageChargeMod = 1f;
        protected float velocityChargeMod = 1f;

        private bool barrageDone = false;

        public float ChargedAmmo
        {
            get => Projectile.ai[2];
            private set => Projectile.ai[2] = value;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseSplatling;

            muzzleDistance = weaponData.MuzzleOffsetPx;
            chargeTimeThresholds = weaponData.ChargeTimeThresholds;
            barrageMaxAmmo = weaponData.BarrageMaxAmmo;
            barrageVelocity = weaponData.BarrageVelocity;
            barrageShotTime = weaponData.BarrageShotTime;
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            ApplyWeaponInstanceData();

            if (IsThisClientTheProjectileOwner())
            {
                chargeStartAudio = PlayAudio(SoundPaths.ChargeStart.ToSoundStyle(), volume: 0.2f, pitchVariance: 0.1f, maxInstances: 1);
            }
        }

        protected override void ReleaseCharge(Player owner)
        {
            hasFired = true;
            ChargedAmmo = Convert.ToInt32(
                Math.Ceiling(barrageMaxAmmo * (ChargeTime / MaxChargeTime()))
            );
            ChargeTime = barrageShotTime;
            SoundHelper.StopSoundIfActive(chargeStartAudio);

            // Set the damage modifier
            switch (chargeLevel)
            {
                case 0:
                    damageChargeMod = 0.6f;
                    velocityChargeMod = 0.6f;
                    barrageShotTime = (int)(barrageShotTime * 1.2f);
                    ChargedAmmo = Math.Min(barrageMaxAmmo - 1, ChargedAmmo);
                    break;
                case 1:
                    damageChargeMod = 1f;
                    velocityChargeMod = 1f;
                    ChargedAmmo = Math.Min(barrageMaxAmmo, ChargedAmmo);
                    break;
            }
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (IsThisClientTheProjectileOwner() && !barrageDone)
            {
                if (owner.dead || owner.GetModPlayer<SquidPlayer>().IsSquid())
                {
                    Projectile.Kill();
                    return;
                }

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

                    AllowChargeCancel();
                    if (chargeCanceled) return;

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
                            position: owner.Center + spawnPositionOffset,
                            velocity: velocity,
                            type: ProjectileType,
                            damage: Convert.ToInt32(Projectile.damage * damageChargeMod));

                        for (int i = 0; i < 15; i++)
                        {
                            Color dustColor = GenerateInkColor();

                            float random = Main.rand.NextFloat(-5, 5);
                            float velX = ((velocity.X + random) * 0.5f);
                            float velY = ((velocity.Y + random) * 0.5f);

                            Dust.NewDust(owner.Center + spawnPositionOffset, 1, 1, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
                        }

                        // Sync shoot animation
                        // Set item use animation and angle
                        NetUpdate(ProjNetUpdateType.ShootAnimation);
                    }

                    return;
                }

                if (!barrageDone)
                {
                    barrageDone = true;
                    Projectile.timeLeft = 60;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!GetOwner().channel) return false;
            DrawStraightTrajectoryLine();
            return false;
        }

        protected override void ChargeLevelUpEffect()
        {
            base.ChargeLevelUpEffect();
            if (IsChargeMaxedOut())
            {
                CreateChildProjectile<WeaponChargeSparkleVisual>(Owner.Center, Vector2.Zero, 0, true);
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
