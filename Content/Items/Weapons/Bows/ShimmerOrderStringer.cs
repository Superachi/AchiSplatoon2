using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class ShimmerOrderStringer : OrderStringer
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
