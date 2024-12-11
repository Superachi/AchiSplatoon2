namespace AchiSplatoon2.Content.Prefixes.ChargeWeaponPrefixes;

internal class BacklinePrefix : BaseChargeWeaponPrefix
{
    public override float DamageModifier => 0.3f;
    public override float VelocityModifier => 0.3f;
    public override int CritChanceBonus => 10;

    public override float ChargeSpeedModifier => -0.4f;
}
