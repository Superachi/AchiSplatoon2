using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Projectiles;

namespace AchiSplatoon2.Content.Items.Weapons.Test
{
    internal class SpecialChargeGun : BaseWeapon
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SpecialChargeProjectile>(),
                singleShotTime: 6,
                shotVelocity: 6f);

            Item.damage = 1;
        }
    }
}
