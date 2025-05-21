using AchiSplatoon2.Attributes;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    [OrderWeapon]
    internal class OrderBrush : BaseBrush
    {
        public override float AimDeviation { get => 8f; }

        // Brush-specific properties
        public override float ShotVelocity => 6f;
        public override float BaseWeaponUseTime => 25f;
        public override int SwingArc => 120;
        public override float RollMoveSpeedBonus => 1.4f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.damage = 8;
            Item.knockBack = 1f;
            Item.shootSpeed = 4f;

            Item.width = 58;
            Item.height = 58;

            Item.SetValuePreEvilBosses();

            // Note: hide this stat from the player-- the Order Brush shouldn't be seen as a swapout for high-def enemies
            Item.ArmorPenetration = 3;
        }

        public override void AddRecipes() => AddRecipeOrder(ItemID.Sapphire);
    }
}
