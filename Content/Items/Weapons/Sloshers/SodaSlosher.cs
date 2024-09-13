using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class SodaSlosher : Slosher
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 100;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipePostPlanteraDungeon();
    }
}
