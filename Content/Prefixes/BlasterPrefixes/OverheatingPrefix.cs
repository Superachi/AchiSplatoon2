namespace AchiSplatoon2.Content.Prefixes.BlasterPrefixes
{
    internal class OverheatingPrefix : BaseBlasterPrefix
    {
        public override float PrefixValueModifier => 1.2f;
        public override float UseTimeModifier => -0.6f;
        public override float InkCostModifier => 3f;
    }
}
