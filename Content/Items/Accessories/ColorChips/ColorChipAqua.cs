using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    internal class ColorChipAqua : ColorChipBase
    {
        protected override int AquaValue => 1;

        public override void AddRecipes()
        {
            ColorChipRecipe(ItemID.CyanDye, ItemID.Diamond)
                .Register();
        }
    }
}
