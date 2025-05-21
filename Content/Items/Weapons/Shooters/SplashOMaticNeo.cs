using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class SplashOMaticNeo : SplashOMatic
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 32;
            Item.shootSpeed = 7f;
            Item.SetValuePostPlantera();
        }
        public override void AddRecipes() => AddRecipePostPlanteraDungeon();
    }
}
