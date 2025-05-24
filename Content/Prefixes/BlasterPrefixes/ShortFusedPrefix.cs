namespace AchiSplatoon2.Content.Prefixes.BlasterPrefixes
{
    internal class ShortFusedPrefix : BaseBlasterPrefix
    {
        public override float PrefixValueModifier => 1.2f;
        public override float UseTimeModifier => -0.2f;
        public override float ExplosionRadiusModifier => -0.2f;
        public override float LifetimeModifier => -0.5f;
        public override float InkCostModifier => -0.2f;
    }
}
