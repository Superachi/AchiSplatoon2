using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Projectiles.SlosherProjectiles;
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

            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes() => AddRecipeOrder(ItemID.Amethyst);
    }
}
