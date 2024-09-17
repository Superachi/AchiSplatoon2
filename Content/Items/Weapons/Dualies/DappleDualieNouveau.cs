using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class DappleDualieNouveau : DappleDualie
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 60;
            Item.shootSpeed = 4;
            Item.crit = 5;
            Item.knockBack = 4;
            Item.damage = 40;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipePostPlanteraDungeon();
    }
}
