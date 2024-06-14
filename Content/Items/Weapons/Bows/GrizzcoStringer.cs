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
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class GrizzcoStringer : TriStringer
    {
        public override float[] ChargeTimeThresholds { get => [40f, 80f]; }
        public override float ShotgunArc { get => 12f; }
        public override int ProjectileCount { get => 9; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToRangedWeapon(
                baseProjType: ModContent.ProjectileType<TriStringerCharge>(),
                ammoID: AmmoID.None,
                singleShotTime: 30,
                shotVelocity: 4f);

            Item.damage = 92;
            Item.width = 34;
            Item.height = 74;
            Item.knockBack = 4;
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
            recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
