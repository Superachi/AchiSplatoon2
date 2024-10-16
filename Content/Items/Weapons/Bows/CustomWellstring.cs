using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class CustomWellstring : Wellstring
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 150;

            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }
    }
}
