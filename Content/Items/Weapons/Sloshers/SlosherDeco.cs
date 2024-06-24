using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class SlosherDeco : Slosher
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 114;
            Item.width = 32;
            Item.height = 32;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.PalladiumBar, 5);
            recipe.Register();
        }
    }
}
