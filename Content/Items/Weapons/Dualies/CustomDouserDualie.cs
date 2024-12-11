using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class CustomDouserDualie : DouserDualie
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 36;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes()
        {
        }
    }
}
