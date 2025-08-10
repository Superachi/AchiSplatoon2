using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Projectiles.BlasterProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    [ShimmerOrderWeapon]
    internal class ShimmerOrderBlaster : OrderBlaster
    {
        public override float InkCost { get => 0f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BlasterProjectile>(),
                singleShotTime: 50,
                shotVelocity: 6f);

            Item.SetValuePostEvilBosses();
        }

        public override void AddRecipes() { }
    }
}
