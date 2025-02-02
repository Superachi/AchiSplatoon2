using AchiSplatoon2.Content.Projectiles.SlosherProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class OrderSlosher : BaseSlosher
    {
        public override float InkCost { get => 12f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SlosherMainProjectile>(),
                singleShotTime: 32,
                shotVelocity: 6f
            );
            Item.useStyle = ItemUseStyleID.DrinkLiquid;

            Item.damage = 12;
            Item.crit = 0;
            Item.knockBack = 2;

            Item.width = 32;
            Item.height = 32;

            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes() => AddRecipeOrder();
    }
}
