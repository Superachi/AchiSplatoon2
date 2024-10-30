using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    internal class KensaSplatCharger : SplatCharger
    {
        public override float[] ChargeTimeThresholds { get => [48f]; }
        public override bool SlowAerialCharge { get => false; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 260;
            Item.crit = 10;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
