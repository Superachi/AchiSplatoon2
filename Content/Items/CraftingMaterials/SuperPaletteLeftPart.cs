using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.CraftingMaterials
{
    internal class SuperPaletteLeftPart : BaseItem
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.SetValueHighHardmodeOre();
        }
    }
}
