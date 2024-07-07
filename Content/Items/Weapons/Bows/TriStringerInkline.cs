using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class TriStringerInkline : TriStringer
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 82;
            Item.knockBack = 6;
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
