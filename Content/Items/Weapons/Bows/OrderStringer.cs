using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class OrderStringer : TriStringer
    {
        public override float ShotgunArc { get => 6f; }
        public override int ProjectileCount { get => 3; }
        public override bool AllowStickyProjectiles { get => false; }
        public override Vector2? HoldoutOffset() { return new Vector2(-4, 2); }
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 32;
            Item.height = 74;
            Item.damage = 14;
            Item.knockBack = 2f;
            Item.crit = 0;
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
    }
}
