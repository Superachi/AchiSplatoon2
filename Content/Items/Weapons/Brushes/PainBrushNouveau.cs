using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class PainBrushNouveau : PainBrush
    {
        public override float BaseWeaponUseTime => 16f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.damage = 68;
            Item.SetValuePostPlantera();
        }

        public override void AddRecipes() => AddRecipePostPlanteraDungeon();
    }
}
