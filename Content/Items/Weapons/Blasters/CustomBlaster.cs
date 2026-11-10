using AchiSplatoon2.Content.Projectiles.BlasterProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    internal class CustomBlaster : Blaster
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BlasterProjectile>(),
                singleShotTime: 50,
                shotVelocity: 7f);

            // Between Luna Blaster (120 @ 40 frames) and Range Blaster (160 @ 58 frames),
            // interpolated by firing speed (50 frames).
            Item.damage = 142;
            Item.knockBack = 5;

            Item.SetValueMidHardmodeOre();
        }

        public override void AddRecipes()
        {
        }
    }
}
