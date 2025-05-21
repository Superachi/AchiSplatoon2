using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class KillerWail : BaseSpecial
    {
        public override bool IsDurationSpecial => true;
        public override float SpecialDrainPerTick => 0.235f;
        public override float RechargeCostPenalty { get => 80f; }

        private static readonly int ArmorPierce = 10;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ArmorPierce);

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 50;
            Item.knockBack = 1;
            Item.ArmorPenetration = ArmorPierce;

            Item.width = 24;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTurn = true;
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true);
    }
}
