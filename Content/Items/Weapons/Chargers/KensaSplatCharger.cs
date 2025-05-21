using AchiSplatoon2.ExtensionMethods;
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
            Item.damage = 240;
            Item.crit = 10;
            Item.knockBack = 6;
            Item.SetValuePostPlantera();
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
