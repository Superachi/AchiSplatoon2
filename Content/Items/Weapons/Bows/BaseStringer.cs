using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class BaseStringer : BaseWeapon
    {
        public virtual float[] ChargeTimeThresholds { get => [36f, 72f]; }
        public override string ShootSample { get => "TriStringerShoot"; }
        public override string ShootWeakSample { get => "BambooChargerShootWeak"; }
        public virtual float ShotgunArc { get => 4f; }
        public virtual int ProjectileCount { get => 3; }
        public virtual bool AllowStickyProjectiles { get => true; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<TriStringerCharge>(),
                singleShotTime: 18,
                shotVelocity: 0);

            Item.noMelee = true;
            Item.channel = true;
            Item.crit = 5;
        }
    }
}
