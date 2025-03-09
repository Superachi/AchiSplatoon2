namespace AchiSplatoon2.Content.Prefixes.SlosherPrefixes
{
    internal class ResonantPrefix : BaseSlosherPrefix
    {
        public override float PrefixValueModifier => 2f;
        public override float DamageModifier => 0f;
        public override int RepetitionBonus => 1;
        public override float VelocityModifier => -0.2f;
        public override float UseTimeModifier => 0.4f;
        public override float InkCostModifier => 0.5f;
    }
}
