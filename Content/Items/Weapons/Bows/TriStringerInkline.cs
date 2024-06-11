using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Items.Weapons.Blasters;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class TriStringerInkline : TriStringer
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 90;
            Item.knockBack = 8;
            Item.value = Item.buyPrice(gold: 25);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TriStringer>());
            recipe.AddIngredient(ItemID.CobaltBar, 12);
            recipe.AddIngredient(ItemID.CrystalShard, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
