using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class ReefluxDecoStringer : ReefluxStringer
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 76;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(gold: 15);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ReefluxStringer>());
            recipe.AddIngredient(ItemID.MythrilBar, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
