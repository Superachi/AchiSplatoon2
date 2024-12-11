namespace AchiSplatoon2.Content.Prefixes.GeneralPrefixes;

internal class ChaoticPrefix : BaseWeaponPrefix
{
    public override float UseTimeModifier => -0.2f;
    public override int CritChanceBonus => 15;
    public override int AimVariation => 20;
    public override float InkCostModifier => -0.1f;
}
