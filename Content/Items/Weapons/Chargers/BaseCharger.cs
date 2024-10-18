using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    internal class BaseCharger : BaseWeapon
    {
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Charger;

        public virtual float[] ChargeTimeThresholds { get => [60f]; }
        public virtual bool ScreenShake { get => true; }
        public virtual int MaxPenetrate { get => 10; }
        public virtual bool DirectHitEffect { get => true; }
        public virtual float RangeModifier { get => 1f; }
        public virtual float MinPartialRange { get => 0.1f; }
        public virtual float MaxPartialRange { get => 0.4f; }

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
