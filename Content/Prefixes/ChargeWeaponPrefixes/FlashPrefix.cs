namespace AchiSplatoon2.Content.Prefixes.ChargeWeaponPrefixes
{
    internal class FlashPrefix : BaseChargeWeaponPrefix
    {
        public override float PrefixValueModifier => 1.8f;
        public override float DamageModifier => -0.5f;
        public override float ChargeSpeedModifier => 2f;
        public override float InkCostModifier => -0.3f;
    }
}
