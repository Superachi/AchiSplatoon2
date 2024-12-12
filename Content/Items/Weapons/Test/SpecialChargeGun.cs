using AchiSplatoon2.Content.Projectiles;
using Terraria;
using Terraria.ModLoader;

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
