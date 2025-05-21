using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class RecycleBrellaMk2 : RecycleBrella
    {
        public override int ShieldLife => 200;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 52;
            Item.SetValuePostPlantera();
        }

        public override void AddRecipes() => AddRecipePostPlanteraDungeon();
    }
}
