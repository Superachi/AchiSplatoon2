using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class SplooshOMaticNeo : SplooshOMatic
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 40;
            Item.shootSpeed = 4.5f;
            Item.SetValuePostPlantera();
        }
        public override void AddRecipes() => AddRecipePostPlanteraDungeon();
    }
}
