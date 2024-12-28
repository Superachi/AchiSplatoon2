namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class BombRush : BaseSpecial
    {
        public override float SpecialDrainPerTick => 0.1285f;
        public override float SpecialDrainPerUse => 0f;
        public override float RechargeCostPenalty { get => 120f; }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = -1;
            Item.width = 58;
            Item.height = 56;
        }
    }
}
