using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class EnperrySplatDualie : SplatDualie
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 38;
            Item.width = 50;
            Item.height = 36;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(gold: 25);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.Register();
        }
    }
}
