using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class RecycleBrellaMk2 : RecycleBrella
    {
        public override int ShieldLife => 200;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 52;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipePostPlanteraDungeon();
    }
}
