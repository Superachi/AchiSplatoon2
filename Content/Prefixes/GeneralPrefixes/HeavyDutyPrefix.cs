namespace AchiSplatoon2.Content.Prefixes.GeneralPrefixes
{
    internal class HeavyDutyPrefix : BaseWeaponPrefix
    {
        public override float PrefixValueModifier => 0.6f;
        public override float DamageModifier => 1.2f;
        public override float KnockbackModifier => 0.2f;
        public override float UseTimeModifier => 1f;
        public override float VelocityModifier => 0.2f;
    }
}
