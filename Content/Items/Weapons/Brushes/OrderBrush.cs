using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class OrderBrush : BaseBrush
    {
        public override float AimDeviation { get => 8f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 8;
            Item.knockBack = 2f;
            Item.shootSpeed = 6f;

            Item.useTime = 18;
            Item.useAnimation = Item.useTime;

            Item.width = 58;
            Item.height = 58;

            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("IronBar", 5);
            recipe.AddIngredient(ItemID.Gel, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

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
