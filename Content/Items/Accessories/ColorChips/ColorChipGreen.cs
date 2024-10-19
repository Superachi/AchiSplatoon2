using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    internal class ColorChipGreen : ColorChipBase
    {
        protected override int GreenValue => 1;

        public override void AddRecipes()
        {
            ColorChipRecipe(ItemID.GreenDye, ItemID.Emerald)
                .Register();

            ColorChipRecipe(ItemID.LimeDye, ItemID.Emerald)
                .Register();
        }
    }
}
