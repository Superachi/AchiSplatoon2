using AchiSplatoon2.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    public class Splattershot : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 42;
            Item.height = 26;
            Item.useTime = 8;
            Item.useAnimation = Item.useTime;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<SplattershotProjectile>();
            Item.shootSpeed = 30;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<OrderShot>());
            recipe.AddIngredient(ItemID.DemoniteBar, 10);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe altRecipe = CreateRecipe();
            altRecipe.AddIngredient(ModContent.ItemType<OrderShot>());
            altRecipe.AddIngredient(ItemID.CrimtaneBar, 10);
            altRecipe.AddIngredient(ItemID.IllegalGunParts, 1);
            altRecipe.AddTile(TileID.Anvils);
            altRecipe.Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 2);
        }
    }
}
