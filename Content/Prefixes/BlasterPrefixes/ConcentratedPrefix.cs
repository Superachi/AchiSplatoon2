namespace AchiSplatoon2.Content.Prefixes.BlasterPrefixes
{
    internal class ConcentratedPrefix : BaseBlasterPrefix
    {
        public override float DamageModifier => 0.2f;
        public override int CritChanceBonus => 10;
        public override float ExplosionRadiusModifier => -0.5f;
    }
}
