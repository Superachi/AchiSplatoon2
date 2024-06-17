using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    internal class HyperfreshPalette : ChipPalette
    {
        protected override int PaletteCapacity { get => 8; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 32;
            Item.height = 28;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
        }
    }
}
