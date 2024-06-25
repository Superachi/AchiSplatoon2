using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class Octobrush : BaseBrush
    {
        public override float AimDeviation { get => 6f; }
        public override float DelayUntilFall => 12f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 10;
            Item.knockBack = 4;
            Item.shootSpeed = 10f;

            Item.scale = 1.5f;
            Item.useTime = 12;
            Item.useAnimation = Item.useTime;

            Item.width = 60;
            Item.height = 60;

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseBasic(registerNow: false);
            recipe.AddIngredient(ItemID.MeteoriteBar, 5);
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
