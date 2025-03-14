namespace AchiSplatoon2.Content.Prefixes.SlosherPrefixes
{
    internal class OversizedPrefix : BaseSlosherPrefix
    {
        public override float PrefixValueModifier => 1f;
        public override float DamageModifier => 0.2f;
        public override float UseTimeModifier => 0.3f;
        public override int AimVariation => 10;
        public override float VelocityModifier => 0.1f;
        public override int AmmoBonus => 1;
    }
}
