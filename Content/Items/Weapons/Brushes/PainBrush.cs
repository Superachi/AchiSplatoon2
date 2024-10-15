using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class PainBrush : BaseBrush
    {
        public override float AimDeviation { get => 3f; }
        public override float DelayUntilFall => 20f;

        // Brush-specific properties
        public override float ShotVelocity => 9f;
        public override float ShotGravity => 0.1f;
        public override float BaseWeaponUseTime => 18f;
        public override int SwingArc => 160;
        public override int WindupTime => 26;
        public override float RollMoveSpeedBonus => 1.4f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.damage = 62;
            Item.knockBack = 8;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true);
    }
}
