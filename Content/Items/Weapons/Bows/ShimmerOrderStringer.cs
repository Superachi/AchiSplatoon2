using AchiSplatoon2.Attributes;
using AchiSplatoon2.ExtensionMethods;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    [ShimmerOrderWeapon]
    internal class ShimmerOrderStringer : OrderStringer
    {
        public override float InkCost { get => 0f; }
        public virtual float[] ChargeTimeThresholds { get => [30f, 60f]; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 16;

            Item.SetValuePostEvilBosses();
        }

        public override void AddRecipes() { }
    }
}
