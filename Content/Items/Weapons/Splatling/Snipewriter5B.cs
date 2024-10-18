using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class Snipewriter5B : Snipewriter5H
    {
        public override int BarrageShotTime { get; set; } = 12;
        public override int BarrageMaxAmmo { get; set; } = 8;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 180;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipePostPlanteraDungeon();
    }
}
