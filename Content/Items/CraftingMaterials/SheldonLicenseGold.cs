using AchiSplatoon2.ExtensionMethods;
using Terraria;

namespace AchiSplatoon2.Content.Items.CraftingMaterials
{
    internal class SheldonLicenseGold : SheldonLicense
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.SetValuePostMech();
        }
    }
}
