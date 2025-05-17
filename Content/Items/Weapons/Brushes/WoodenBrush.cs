using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class WoodenBrush : BaseBrush
    {
        public override float InkCost { get => 4f; }
        public override float AimDeviation { get => 12f; }

        // Brush-specific properties
        public override float ShotVelocity => 4f;
        public override float BaseWeaponUseTime => 20f;
        public override int SwingArc => 100;
        public override float RollMoveSpeedBonus => 1.4f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.damage = 4;
            Item.knockBack = 0f;

            Item.width = 52;
            Item.height = 52;

            Item.value = Item.buyPrice(silver: 5);
            Item.rare = ItemRarityID.White;

            // Note: hide this stat from the player--
            Item.ArmorPenetration = 8;
        }

        public override void AddRecipes() => AddRecipeWood();
    }
}
