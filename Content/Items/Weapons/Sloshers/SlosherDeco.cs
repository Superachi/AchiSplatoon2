using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class SlosherDeco : Slosher
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 70;
            Item.width = 32;
            Item.height = 32;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.MythrilBar, 5);
            recipe.Register();
        }
    }
}
