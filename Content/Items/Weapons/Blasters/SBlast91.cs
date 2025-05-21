using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    internal class SBlast91 : SBlast92
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 200;
            Item.SetValuePostPlantera();
        }
        public override void AddRecipes() => AddRecipePostPlanteraDungeon();
    }
}
