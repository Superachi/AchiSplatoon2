using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class LightTetraDualie : DarkTetraDualie
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 32;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes()
        {
        }
    }
}
