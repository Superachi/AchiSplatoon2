using AchiSplatoon2.Attributes;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters
{
    [ItemCategory("Weapon boosting accessories", "MainWeaponBoosters")]
    internal class BaseWeaponBoosterAccessory : BaseAccessory
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.SetValueHighHardmodeOre();
        }
    }
}
