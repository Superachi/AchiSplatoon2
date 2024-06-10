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
    public class SplatCharger : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            Item.DefaultToRangedWeapon(ModContent.ProjectileType<SplatChargerProjectile>(), AmmoID.None, 15, 12f);
            Item.damage = 80;
            Item.width = 82;
            Item.height = 26;
            Item.knockBack = 7;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;
            Item.noMelee = true;
            Item.channel = true;
            Item.crit = 10;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Lens, 6);
            recipe.AddIngredient(ItemID.DemoniteBar, 10);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe altRecipe = CreateRecipe();
            altRecipe.AddIngredient(ItemID.Lens, 6);
            altRecipe.AddIngredient(ItemID.CrimtaneBar, 10);
            altRecipe.AddIngredient(ItemID.IllegalGunParts, 1);
            altRecipe.AddTile(TileID.Anvils);
            altRecipe.Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 2);
        }
    }
}
