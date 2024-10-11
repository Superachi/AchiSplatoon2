using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class Octobrush : BaseBrush
    {
        public override float AimDeviation { get => 6f; }
        public override float DelayUntilFall => 12f;
        protected override int ArmorPierce => 10;

        public override float ShotVelocity => 9f;
        public override float BaseWeaponUseTime => 15f;
        public override int SwingArc => 120;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 20;
            Item.knockBack = 5;
            Item.shootSpeed = 8f;

            Item.scale = 1f;

            Item.width = 60;
            Item.height = 60;

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes() => AddRecipePostBee();
    }
}
