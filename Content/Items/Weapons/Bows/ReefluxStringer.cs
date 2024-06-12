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
    internal class ReefluxStringer : TriStringer
    {
        public override Vector2? HoldoutOffset() { return new Vector2(-4, 2); }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToRangedWeapon(
                baseProjType: ModContent.ProjectileType<ReefluxStringerCharge>(),
                ammoID: AmmoID.None,
                singleShotTime: 12,
                shotVelocity: 12f);

            Item.width = 36;
            Item.height = 62;
            Item.damage = 12;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 3);
            Item.crit = 16;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldBow);
            recipe.AddIngredient(ItemID.Coral, 3);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe altRecipe = CreateRecipe();
            altRecipe.AddIngredient(ItemID.PlatinumBow);
            altRecipe.AddIngredient(ItemID.Coral, 3);
            altRecipe.AddTile(TileID.Anvils);
            altRecipe.Register();
        }
    }
}
