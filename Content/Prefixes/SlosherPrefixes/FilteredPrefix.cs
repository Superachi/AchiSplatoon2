namespace AchiSplatoon2.Content.Prefixes.SlosherPrefixes
{
    internal class FilteredPrefix : BaseSlosherPrefix
    {
        public override float PrefixValueModifier => 1f;
        public override float DamageModifier => 0.2f;
        public override int CritChanceBonus => 10;
        public override float UseTimeModifier => -0.2f;
        public override float InkCostModifier => -0.2f;
        public override int AmmoBonus => -2;
    }
}
