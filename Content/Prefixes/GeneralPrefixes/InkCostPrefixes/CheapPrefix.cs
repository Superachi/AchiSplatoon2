namespace AchiSplatoon2.Content.Prefixes.GeneralPrefixes.InkCostPrefixes
{
    internal class CheapPrefix : BaseWeaponPrefix
    {
        public override float PrefixValueModifier => 1.5f;
        public override float DamageModifier => -0.2f;
        public override float InkCostModifier => -0.5f;
    }
}
