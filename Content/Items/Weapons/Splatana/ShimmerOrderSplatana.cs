using AchiSplatoon2.Attributes;
using AchiSplatoon2.ExtensionMethods;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    [ShimmerOrderWeapon]
    internal class ShimmerOrderSplatana : OrderSplatana
    {
        public override float InkCost { get => 0f; }
        public override float[] ChargeTimeThresholds { get => [40f]; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.SetValuePostEvilBosses();
        }

        public override void AddRecipes() { }
    }
}
