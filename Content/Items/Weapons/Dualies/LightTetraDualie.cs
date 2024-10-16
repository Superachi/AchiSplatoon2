using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class LightTetraDualie : DarkTetraDualie
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 40;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }
    }
}
