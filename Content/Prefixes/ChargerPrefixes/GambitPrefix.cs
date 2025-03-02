namespace AchiSplatoon2.Content.Prefixes.ChargerPrefixes
{
    internal class GambitPrefix : BaseChargerPrefix
    {
        public override float DamageModifier => 0.5f;
        public override int CritChanceBonus => 20;
        public override float ChargeSpeedModifier => 0.2f;
        public override int AimVariation => 8;
    }
}
