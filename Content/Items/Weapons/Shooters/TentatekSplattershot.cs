using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class TentatekSplattershot : Splattershot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.useTime = 6;
            Item.useAnimation = Item.useTime;
            Item.damage = 24;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(gold: 8);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ModContent.ItemType<Splattershot>());
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.CrystalShard, 10);
            recipe.Register();
        }
    }
}
