using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters
{
    internal class BaseWeaponBoosterAccessory : BaseAccessory
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.value = Item.buyPrice(gold: 7, silver: 50);
            Item.rare = ItemRarityID.Pink;
        }
    }
}
