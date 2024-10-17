using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    internal class ColorChipYellow : ColorChipBase
    {
        protected override int YellowValue => 1;

        public override void AddRecipes()
        {
            ColorChipRecipe(ItemID.YellowDye, ItemID.Topaz)
                .Register();
        }
    }
}
