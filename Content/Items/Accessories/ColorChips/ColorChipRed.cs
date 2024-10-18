using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    internal class ColorChipRed : ColorChipBase
    {
        protected override int RedValue => 1;

        public override void AddRecipes()
        {
            ColorChipRecipe(ItemID.RedDye, ItemID.Ruby)
                .Register();
        }
    }
}
