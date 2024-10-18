using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class SplashOMaticNeo : SplashOMatic
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 44;
            Item.shootSpeed = 7f;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes() => AddRecipePostPlanteraDungeon();
    }
}
