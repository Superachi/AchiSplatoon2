namespace AchiSplatoon2.Content.Prefixes.GeneralPrefixes
{
    internal class HeavyDutyPrefix : BaseWeaponPrefix
    {
        public override float PrefixValueModifier => 0.5f;
        public override float DamageModifier => 2.5f;
        public override float KnockbackModifier => 1.2f;
        public override float UseTimeModifier => 2f;
        public override float VelocityModifier => 1.2f;
    }
}
