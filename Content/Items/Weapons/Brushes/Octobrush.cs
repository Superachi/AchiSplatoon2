using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class Octobrush : BaseBrush
    {
        public override float AimDeviation { get => 6f; }
        public override float DelayUntilFall => 8f;
        protected override int ArmorPierce => 10;

        public override float ShotVelocity => 8f;
        public override float ShotGravity => 0.2f;
        public override float BaseWeaponUseTime => 15f;
        public override int SwingArc => 130;
        public override float RollMoveSpeedBonus => 1.85f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.damage = 14;
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
