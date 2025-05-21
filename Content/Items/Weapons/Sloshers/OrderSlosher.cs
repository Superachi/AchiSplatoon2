using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Projectiles.SlosherProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    [OrderWeapon]
    internal class OrderSlosher : BaseSlosher
    {
        public override float InkCost { get => 10f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SlosherMainProjectile>(),
                singleShotTime: 40,
                shotVelocity: 6.5f
            );
            Item.useStyle = ItemUseStyleID.DrinkLiquid;

            Item.damage = 16;
            Item.knockBack = 4;

            Item.width = 32;
            Item.height = 32;

            Item.SetValuePreEvilBosses();
        }

        public override void AddRecipes() => AddRecipeOrder(ItemID.Amethyst);
    }
}
