using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class TentatekSplattershot : Splattershot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToRangedWeapon(
                baseProjType: ModContent.ProjectileType<SplattershotProjectile>(),
                ammoID: AmmoID.None,
                singleShotTime: 6,
                shotVelocity: 8f);

            Item.damage = 36;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Splattershot>());
            recipe.AddIngredient(ItemID.PinkGel, 10);
            recipe.AddIngredient(ItemID.CrystalShard, 10);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
