namespace AchiSplatoon2.Content.Prefixes.GeneralPrefixes;

internal class FreshPrefix : BaseWeaponPrefix
{
    public override float DamageModifier => 0.05f;
    public override float UseTimeModifier => -0.1f;
    public override int CritChanceBonus => 5;
    public override float InkCostModifier => -0.1f;
}
