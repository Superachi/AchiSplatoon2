using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class ShimmerOrderSplatana : OrderSplatana
    {
        public override float InkCost { get => 0f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
        }
    }
}
