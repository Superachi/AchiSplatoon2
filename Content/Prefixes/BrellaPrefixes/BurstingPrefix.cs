namespace AchiSplatoon2.Content.Prefixes.BrellaPrefixes
{
    internal class BurstingPrefix : BaseBrellaPrefix
    {
        public override float PrefixValueModifier => 1.2f;
        public override float InkCostModifier => 0.3f;
        public override int AimVariation => 10;
        public override float LifetimeModifier => -0.9f;
        public override int ExtraProjectileBonus => 2;
    }
}
