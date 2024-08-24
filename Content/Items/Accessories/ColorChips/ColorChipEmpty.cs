using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    internal class ColorChipEmpty : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = Item.CommonMaxStack;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Glass, 10);
            recipe.AddIngredient(ItemID.Gel, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
