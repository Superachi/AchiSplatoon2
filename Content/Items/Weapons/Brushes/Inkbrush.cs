using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Projectiles.BrushProjectiles;
using AchiSplatoon2.Content.Players;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class Inkbrush : BaseBrush
    {
        protected override int ArmorPierce => 5;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 6;
            Item.knockBack = 3;

            Item.scale = 1;
            Item.useTime = 8;
            Item.useAnimation = Item.useTime;

            Item.width = 56;
            Item.height = 64;

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseBasic(registerNow: false);
            recipe.AddIngredient(ItemID.MeteoriteBar, 5);
            recipe.Register();
        }
    }
}
