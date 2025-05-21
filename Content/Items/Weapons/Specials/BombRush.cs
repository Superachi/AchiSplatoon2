namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class BombRush : BaseSpecial
    {
        public override float SpecialDrainPerTick => 0.1285f;
        public override float SpecialDrainPerUse => 0f;
        public override float RechargeCostPenalty { get => 120f; }

        public static float SubWeaponDamageMultiplier => 1.1f;
        protected override string UsageHintParamA => $"{(int)((SubWeaponDamageMultiplier - 1) * 100)}";

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = -1;
            Item.width = 58;
            Item.height = 56;
        }
    }
}
