using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    [ItemCategory("Stringer", "Bows")]
    internal class BaseStringer : BaseWeapon
    {
        public override float InkCost { get => 1.5f; }
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Stringer;

        public virtual float[] ChargeTimeThresholds { get => [36f, 72f]; }
        public override SoundStyle ShootSample { get => SoundPaths.TriStringerShoot.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.TriStringerShootWeak.ToSoundStyle(); }
        public virtual float ShotgunArc { get => 5f; }
        public virtual int ProjectileCount { get => 3; }
        public virtual bool AllowStickyProjectiles { get => true; }

        public virtual int ProjectileType => ModContent.ProjectileType<TriStringerProjectile>();

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<TriStringerCharge>(),
                singleShotTime: 18,
                shotVelocity: 1);

            Item.noMelee = true;
            Item.channel = true;
            Item.crit = 5;
        }
    }
}
