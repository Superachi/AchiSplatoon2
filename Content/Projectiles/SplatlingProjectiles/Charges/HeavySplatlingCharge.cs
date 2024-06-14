using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using AchiSplatoon2.Content.Items.Weapons;
using Terraria.DataStructures;

namespace AchiSplatoon2.Content.Projectiles.SplatlingProjectiles.Charges
{
    internal class HeavySplatlingCharge : BaseChargeProjectile
    {
        protected override string ShootSample { get => "SplattershotShoot"; }
        protected override float[] ChargeTimeThresholds { get => [50f, 75f]; }
        protected virtual int ProjectileType { get => ModContent.ProjectileType<HeavySplatlingProjectile>(); }
        private float barrageMaxAmmo;
        private float barrageVelocity;
        private float barrageShotTime;
        private float muzzleDistance;
        private float damageChargeMod = 1f;
        private float velocityChargeMod = 1f;

        protected float ChargedAmmo
        {
            get => Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            HeavySplatling weaponData = new HeavySplatling();
            muzzleDistance = weaponData.MuzzleOffsetPx;
            barrageMaxAmmo = weaponData.BarrageMaxAmmo;
            barrageVelocity = weaponData.BarrageVelocity;
            barrageShotTime = weaponData.BarrageShotTime;
        }

        protected override void ReleaseCharge(Player owner)
        {
            hasFired = true;
            ChargedAmmo = Convert.ToInt32(barrageMaxAmmo * (ChargeTime / MaxChargeTime()));
            ChargeTime = 0;
            PlayAudio(soundPath: "ChargeStart", volume: 0f, maxInstances: 1);

            // Set the damage modifier
            switch (chargeLevel)
            {
                case 0:
                    damageChargeMod = 0.6f;
                    velocityChargeMod = 0.6f;
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

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (IsThisClientTheProjectileOwner())
            {
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

                    ChargeTime += 1f * attackSpeedModifier;
                    if (ChargeTime >= barrageShotTime)
                    {
                        // If ChargeTime were directly set to 0, it would not work nicely with non-decimal values (eg. when attack speed is increased)
                        ChargeTime -= barrageShotTime;
                        ChargedAmmo--;
                        PlayerItemAnimationFaceCursor(owner);

                        // Calculate angle/velocity
                        float aimAngle = MathHelper.ToDegrees(
                            owner.DirectionTo(Main.MouseWorld).ToRotation()
                        );

                        float radians = MathHelper.ToRadians(aimAngle);
                        Vector2 angleVector = new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
                        Vector2 velocity = angleVector * barrageVelocity * velocityChargeMod;

                        // Spawn the projectile
                        var spawnPositionOffset = Vector2.Normalize(velocity) * muzzleDistance;

                        if (!Collision.CanHit(Projectile.position, 0, 0, Projectile.position + spawnPositionOffset, 0, 0))
                        {
                            spawnPositionOffset = Vector2.Zero;
                        }

                        int proj = Projectile.NewProjectile(
                        spawnSource: Projectile.GetSource_FromThis(),
                        position: Projectile.position + spawnPositionOffset,
                        velocity: velocity,
                        Type: ProjectileType,
                        Damage: Convert.ToInt32(Projectile.damage * damageChargeMod),
                        KnockBack: Projectile.knockBack,
                        Owner: Main.myPlayer);
                    }

                    return;
                }

                Projectile.Kill();
            }
        }
    }
}
