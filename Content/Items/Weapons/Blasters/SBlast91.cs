using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    internal class SBlast91 : SBlast92
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 190;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes() => AddRecipePostPlanteraDungeon();
    }
}
