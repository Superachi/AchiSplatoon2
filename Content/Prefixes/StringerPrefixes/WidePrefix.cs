namespace AchiSplatoon2.Content.Prefixes.StringerPrefixes;

internal class WidePrefix : BaseStringerPrefix
{
    public override float PrefixValueModifier => 1.5f;
    public override float DamageModifier => -0.2f;
    public override int ExtraProjectileBonus => 2;
    public override float ShotgunArcModifier => 0.6f;
    public override float InkCostModifier => 0.2f;
}
