namespace AchiSplatoon2.Content.Prefixes.SplatlingPrefixes
{
    internal class CuratedPrefix : BaseSplatlingPrefix
    {
        public override float DamageModifier => 0.4f;
        public override int CritChanceBonus => 10;
        public override float ChargeSpeedModifier => 0.2f;
        public override float ShotsPerChargeModifier => -0.5f;
        public override float ShotSpreadModifier => -0.3f;
    }
}
