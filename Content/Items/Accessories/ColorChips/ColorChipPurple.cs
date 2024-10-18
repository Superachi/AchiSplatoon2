using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    internal class ColorChipPurple : ColorChipBase
    {
        protected override int PurpleValue => 1;

        public override void AddRecipes()
        {
            ColorChipRecipe(ItemID.PurpleDye, ItemID.Amethyst)
                .Register();
        }
    }
}
