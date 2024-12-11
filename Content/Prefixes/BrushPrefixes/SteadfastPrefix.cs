namespace AchiSplatoon2.Content.Prefixes.BrushPrefixes
{
    internal class SteadfastPrefix : BaseBrushPrefix
    {
        public override float DamageModifier => 0.3f;
        public override float UseTimeModifier => -0.1f;
        public override int CritChanceBonus => 5;
        public override float VelocityModifier => -0.3f;
        public override float DashSpeedModifier => -0.3f;
    }
}
