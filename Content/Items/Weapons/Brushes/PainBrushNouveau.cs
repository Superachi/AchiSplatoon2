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
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipePostPlanteraDungeon();
    }
}
