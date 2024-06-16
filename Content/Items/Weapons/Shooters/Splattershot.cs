using AchiSplatoon2.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Items.Weapons;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    internal class Splattershot : BaseSplattershot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToRangedWeapon(
                baseProjType: ModContent.ProjectileType<SplattershotProjectile>(),
                ammoID: AmmoID.None,
                singleShotTime: 8,
                shotVelocity: 8f);

            Item.damage = 14;
            Item.width = 42;
            Item.height = 26;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<OrderShot>());
            recipe.AddIngredient(ItemID.DemoniteBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe altRecipe = CreateRecipe();
            altRecipe.AddIngredient(ModContent.ItemType<OrderShot>());
            altRecipe.AddIngredient(ItemID.CrimtaneBar, 10);
            altRecipe.AddTile(TileID.Anvils);
            altRecipe.Register();
        }
    }
}
