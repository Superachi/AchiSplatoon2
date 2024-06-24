using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class ZinkMiniSplatling : MiniSplatling
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 32;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.CobaltBar, 5);
            recipe.Register();
        }
    }
}
