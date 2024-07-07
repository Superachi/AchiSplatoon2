using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.CraftingMaterials
{
    internal class SheldonLicense : BaseItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 18;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
        }
    }
}
