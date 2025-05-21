using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class PainBrush : BaseBrush
    {
        public override float InkCost { get => 4f; }

        public override float AimDeviation { get => 3f; }
        public override float DelayUntilFall => 12f;

        // Brush-specific properties
        public override float ShotVelocity => 9f;
        public override float ShotGravity => 0.15f;
        public override float BaseWeaponUseTime => 18f;
        public override int SwingArc => 160;
        public override int WindupTime => 26;
        public override float RollMoveSpeedBonus => 1.6f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.damage = 44;
            Item.knockBack = 8;
            Item.SetValuePostMech();
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofFright);
    }
}
