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
            ColorChipRecipe(ItemID.RedDye, ItemID.Ruby)
                .Register();
        }
    }
}
