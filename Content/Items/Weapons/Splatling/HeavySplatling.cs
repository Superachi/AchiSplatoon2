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
using AchiSplatoon2.Content.Projectiles.SplatlingProjectiles.Charges;
using AchiSplatoon2.Content.Items.Weapons.Shooters;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class HeavySplatling : BaseWeapon
    {
        public override Vector2? HoldoutOffset() { return new Vector2(-46, 6); }
        public override float MuzzleOffsetPx { get; set; } = 50f;
        public virtual float[] ChargeTimeThresholds { get => [50f, 75f]; }
        public virtual float BarrageVelocity { get; set; } = 12f;
        public virtual int BarrageShotTime { get; set; } = 4;
        public virtual int BarrageMaxAmmo { get; set; } = 32;

        public override void SetDefaults()
        {
            Item.DefaultToRangedWeapon(
                baseProjType: ModContent.ProjectileType<HeavySplatlingCharge>(),
                ammoID: AmmoID.None,
                singleShotTime: BarrageShotTime + 10,
                shotVelocity: BarrageVelocity);
            Item.damage = 20;
            Item.width = 92;
            Item.height = 50;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
            Item.noMelee = true;
            Item.channel = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Minishark, 1);
            recipe.AddIngredient(ItemID.HellstoneBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
