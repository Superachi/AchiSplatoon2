using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class TentatekSplattershot : Splattershot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.useTime = 6;
            Item.useAnimation = Item.useTime;
            Item.damage = 36;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.CrystalShard, 10);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.Register();
        }
    }
}
