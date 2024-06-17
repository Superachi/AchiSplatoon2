using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    internal class ColorChipRed : ColorChipBase
    {
        protected override int RedValue => 1;

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.RedDye);
            recipe.AddIngredient(ModContent.ItemType<ColorChipEmpty>());
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
    }
}
