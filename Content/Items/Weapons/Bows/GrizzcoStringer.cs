using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Projectiles;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class GrizzcoStringer : TriStringer
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToRangedWeapon(
                baseProjType: ModContent.ProjectileType<GrizzcoStringerCharge>(),
                ammoID: AmmoID.None,
                singleShotTime: 30,
                shotVelocity: 4f);

            Item.damage = 92;
            Item.width = 34;
            Item.height = 74;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(gold: 40);
            Item.rare = ItemRarityID.Lime;
            Item.crit = 15;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TriStringer>(), 1);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ItemID.HallowedBar, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
