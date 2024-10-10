using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class Inkbrush : BaseBrush
    {
        public override float AimDeviation { get => 12f; }
        protected override int ArmorPierce => 5;


        // Brush-specific properties
        public override float ShotVelocity => 7f;
        public override float BaseWeaponUseTime => 8f;
        public override int SwingArc => 80;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 9;
            Item.knockBack = 3;

            Item.scale = 1;

            Item.width = 56;
            Item.height = 64;

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes() => AddRecipeMeteorite();
    }
}
