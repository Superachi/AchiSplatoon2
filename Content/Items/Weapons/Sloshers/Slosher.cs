using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class Slosher : BaseSlosher
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 24;
            Item.width = 32;
            Item.height = 32;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseBasic(registerNow: false);
            recipe.AddIngredient(ItemID.MeteoriteBar, 5);
            recipe.Register();
        }
    }
}
