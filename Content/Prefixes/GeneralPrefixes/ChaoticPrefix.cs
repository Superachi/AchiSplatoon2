namespace AchiSplatoon2.Content.Prefixes.GeneralPrefixes;

internal class ChaoticPrefix : BaseWeaponPrefix
{
    public override float UseTimeModifier => -0.2f;
    public override int CritChanceBonus => 12;
    public override int AimVariation => 12;
    public override float InkCostModifier => -0.1f;
}
