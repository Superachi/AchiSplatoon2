using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
