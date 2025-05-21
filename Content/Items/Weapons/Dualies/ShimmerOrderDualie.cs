using AchiSplatoon2.Attributes;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    [ShimmerOrderWeapon]
    internal class ShimmerOrderDualie : OrderDualie
    {
        public override float InkCost { get => 0f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes() { }
    }
}
