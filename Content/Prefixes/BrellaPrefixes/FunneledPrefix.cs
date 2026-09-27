namespace AchiSplatoon2.Content.Prefixes.BrellaPrefixes;

internal class FunneledPrefix : BaseBrellaPrefix
{
    public override float PrefixValueModifier => 1.4f;
    public override float InkCostModifier => -0.3f;
    public override float DamageModifier => -0.2f;
    public override float UseTimeModifier => -0.6f;
    public override int AimVariation => -2;
    public override int ExtraProjectileBonus => -1;
}