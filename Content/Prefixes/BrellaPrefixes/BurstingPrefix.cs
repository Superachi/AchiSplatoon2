namespace AchiSplatoon2.Content.Prefixes.BrellaPrefixes
{
    internal class BurstingPrefix : BaseBrellaPrefix
    {
        public override float UseTimeModifier => 0.5f;
        public override int AimVariation => 10;
        public override float VelocityModifier => 0.3f;
        public override int ExtraProjectileBonus => 3;
    }
}
