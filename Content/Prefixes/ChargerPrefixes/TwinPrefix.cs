namespace AchiSplatoon2.Content.Prefixes.ChargerPrefixes
{
    internal class TwinPrefix : BaseChargerPrefix
    {
        public static float TwinShotArc => 8f;

        public override float PrefixValueModifier => 1.6f;
        public override float DamageModifier => -0.2f;
        public override float LifetimeModifier => -0.6f;
        public override float ChargeSpeedModifier => 0f;
        public override int ExtraProjectileBonus => 1;
        public override float InkCostModifier => 0.2f;
    }
}
