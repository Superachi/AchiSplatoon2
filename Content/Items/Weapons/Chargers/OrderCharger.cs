using AchiSplatoon2.Attributes;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    [OrderWeapon]
    internal class OrderCharger : SplatCharger
    {
        public override float[] ChargeTimeThresholds { get => [75f]; }

        public override float RangeModifier => 0.4f;
        public override float MinPartialRange { get => 0.3f; }
        public override float MaxPartialRange { get => 0.6f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 28;
            Item.knockBack = 4;
            Item.crit = 0;
            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes() => AddRecipeOrder(ItemID.Amethyst);
    }
}
