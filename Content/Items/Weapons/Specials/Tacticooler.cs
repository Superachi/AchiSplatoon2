using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class Tacticooler : BaseSpecial
    {
        public override bool IsDurationSpecial => true;
        public override float SpecialDrainPerTick => 0.05f;
        public override float RechargeCostPenalty { get => 120f; }

        // public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs();

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 0;
            Item.knockBack = 0;
            Item.width = 36;
            Item.height = 20;
        }
    }
}
