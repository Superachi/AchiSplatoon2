namespace AchiSplatoon2.Content.Prefixes.ChargerPrefixes
{
    internal class ExplosivePrefix : BaseChargerPrefix
    {
        public override bool LosePiercing => true;
        public override float DamageModifier => 0.2f;
        public override int CritChanceBonus => 10;
        public override float ChargeSpeedModifier => -0.2f;
    }
}
