using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    internal class KensaSplatCharger : SplatCharger
    {
        public override float[] ChargeTimeThresholds { get => [48f]; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 320;
            Item.crit = 10;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
