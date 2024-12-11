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
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SheldonLicense>())
                .AddIngredient(ItemID.SoulofLight, 10)
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SheldonLicense>())
                .AddIngredient(ItemID.SoulofNight, 10)
                .Register();
        }
    }
}
