using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.CraftingMaterials
{
    internal class SheldonLicenseSilver : SheldonLicense
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.SetValueLowHardmodeOre();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SheldonLicense>())
                .AddIngredient(ItemID.SoulofLight, 3)
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SheldonLicense>())
                .AddIngredient(ItemID.SoulofNight, 3)
                .Register();
        }
    }
}
