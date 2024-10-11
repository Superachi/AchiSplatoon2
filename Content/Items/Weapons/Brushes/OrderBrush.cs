using System;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class OrderBrush : BaseBrush
    {
        public override float AimDeviation { get => 8f; }

        // Brush-specific properties
        public override float ShotVelocity => 6f;
        public override float BaseWeaponUseTime => 15f;
        public override int SwingArc => 120;

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.damage = 8;
            Item.knockBack = 3f;
            Item.shootSpeed = 4f;

            Item.width = 58;
            Item.height = 58;

            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;

            // Note: hide this stat from the player-- the Order Shot shouldn't be seen as a swapout for high-def enemies
            Item.ArmorPenetration = 3;
        }

        public override void AddRecipes() => AddRecipeOrder();

        public override void UseAnimation(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                // Slow down movement while attacking (similar to how the player slows down in Splatoon when attacking with a brush)
                if (Math.Abs(player.velocity.X) > 2f)
                {
                    player.velocity.X *= 0.8f;
                }
            }
        }
    }
}
