namespace AchiSplatoon2.Content.Prefixes.StringerPrefixes;

internal class CompactPrefix : BaseStringerPrefix
{
    public override float DamageModifier => 1f;
    public override int CritChanceBonus => 10;
    public override int ExtraProjectileBonus => -2;
    public override float ShotgunArcModifier => -0.5f;
}
