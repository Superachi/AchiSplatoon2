using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class SplooshOMaticNeo : SplooshOMatic
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 48;
            Item.shootSpeed = 4.5f;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes() => AddRecipePostPlanteraDungeon();
    }
}
