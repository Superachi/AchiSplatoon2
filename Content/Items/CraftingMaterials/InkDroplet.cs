using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.CraftingMaterials
{
    internal class InkDroplet : BaseItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;

            Item.maxStack = Item.CommonMaxStack;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 5);
        }

        public override void AddRecipes()
        {
            CreateRecipe(5)
                .AddIngredient(ItemID.BlackInk, 1)
                .Register();
        }
    }
}
