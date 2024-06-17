using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    internal class ColorChipAqua : ColorChipBase
    {
        protected override int AquaValue => 1;

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CyanDye);
            recipe.AddIngredient(ModContent.ItemType<ColorChipEmpty>());
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
    }
}
