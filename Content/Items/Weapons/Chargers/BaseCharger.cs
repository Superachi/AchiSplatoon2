using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    internal class BaseCharger : BaseWeapon
    {
        public virtual float[] ChargeTimeThresholds { get => [60f]; }

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.noMelee = true;
            Item.channel = true;
            Item.crit = 5;
        }
    }
}
