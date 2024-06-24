using AchiSplatoon2.Content.Projectiles.SlosherProjectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class BaseSlosher : BaseWeapon
    {
        public override string ShootSample { get => "SlosherShoot"; }
        public override string ShootWeakSample { get => "SlosherShootAlt"; }
        public virtual float ShotGravity { get => 0.12f; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToRangedWeapon(
                baseProjType: ModContent.ProjectileType<SlosherMainProjectile>(),
                ammoID: AmmoID.None,
                singleShotTime: 30,
                shotVelocity: 8f);
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
        }
    }
}
