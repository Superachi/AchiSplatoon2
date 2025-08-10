using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Projectiles.SlosherProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    [ShimmerOrderWeapon]
    internal class ShimmerOrderSlosher : OrderSlosher
    {
        public override float InkCost { get => 0f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SlosherMainProjectile>(),
                singleShotTime: 30,
                shotVelocity: 6.5f
            );

            Item.SetValuePostEvilBosses();
        }

        public override void AddRecipes() { }
    }
}
