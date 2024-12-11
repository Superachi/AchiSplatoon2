using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.CraftingMaterials
{
    internal class InkDroplet : BaseItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;

            Item.sellPrice(copper: 10);

            Item.maxStack = Item.CommonMaxStack;
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe(5)
                .AddIngredient(ItemID.BlackInk, 1)
                .Register();
        }
    }
}
