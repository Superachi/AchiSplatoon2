using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class ReefluxDecoStringer : ReefluxStringer
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 76;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 15);
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.MythrilBar, 5);
            recipe.Register();
        }
    }
}
