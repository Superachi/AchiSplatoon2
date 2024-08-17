using System;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class OrderBrush : BaseBrush
    {
        public override float AimDeviation { get => 8f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 7;
            Item.knockBack = 2f;
            Item.shootSpeed = 4f;

            Item.useTime = 24;
            Item.useAnimation = Item.useTime;

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
