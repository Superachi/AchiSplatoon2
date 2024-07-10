using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    internal class ZFSplatCharger : SplatCharger
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 240;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.OrichalcumBar, 5);
            recipe.Register();
        }
    }
}
