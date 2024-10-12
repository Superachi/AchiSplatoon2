using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class PainBrushNouveau : PainBrush
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.damage = 130;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes() => AddRecipePostPlanteraDungeon();
    }
}
