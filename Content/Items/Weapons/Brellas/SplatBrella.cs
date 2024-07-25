using AchiSplatoon2.Content.Projectiles.BrellaProjectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class SplatBrella : BaseBrella
    {
        public override int ProjectileCount { get => 8; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BrellaShotgunProjectile>(),
                singleShotTime: 36,
                shotVelocity: 8f);

            Item.damage = 14;
            Item.width = 50;
            Item.height = 58;
            Item.knockBack = 2;

            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.TitaniumBar, 5);
            recipe.Register();
        }
    }
}
