using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    internal class ColorChipBlue : ColorChipBase
    {
        protected override int BlueValue => 1;

        public override void AddRecipes()
        {
            ColorChipRecipe(ItemID.SkyBlueDye, ItemID.Sapphire)
                .Register();
        }
    }
}
