using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.CraftingMaterials
{
    internal class SheldonLicenseGold : SheldonLicense
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.LightPurple;
        }
    }
}
