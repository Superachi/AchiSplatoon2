namespace AchiSplatoon2.Content.Prefixes.SplatlingPrefixes
{
    internal class ChaoticSplatlingPrefix : BaseSplatlingPrefix
    {
        public override int CritChanceBonus => 5;
        public override float ShotsPerChargeModifier => 0.8f;
        public override int ShotTimeModifier => -1;
        public override float ShotSpreadModifier => 1.2f;
    }
}
