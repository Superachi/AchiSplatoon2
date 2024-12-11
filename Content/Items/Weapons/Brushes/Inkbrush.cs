using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class Inkbrush : BaseBrush
    {
        public override float AimDeviation { get => 12f; }
        protected override int ArmorPierce => 20;


        // Brush-specific properties
        public override float ShotVelocity => 6f;
        public override float BaseWeaponUseTime => 10f;
        public override int SwingArc => 100;
        public override float RollMoveSpeedBonus => 2f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.damage = 6;
            Item.knockBack = 4;
            Item.scale = 1;

            Item.width = 56;
            Item.height = 64;

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes() => AddRecipeMeteorite();
    }
}
