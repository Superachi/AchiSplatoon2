using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class Splattershot : BaseSplattershot
    {
        public override float ShotGravity { get => 0.3f; }
        public override int ShotGravityDelay => 20;
        public override int ShotExtraUpdates { get => 4; }
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 14;
            Item.width = 42;
            Item.height = 26;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseBasic(registerNow: false);
            recipe.AddIngredient(ItemID.DemoniteBar, 5);
            recipe.Register();

            var altRecipe = AddRecipeWithSheldonLicenseBasic(registerNow: false);
            altRecipe.AddIngredient(ItemID.CrimtaneBar, 5);
            altRecipe.Register();
        }
    }
}
