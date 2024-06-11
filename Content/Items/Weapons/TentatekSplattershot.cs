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
    public class TentatekSplattershot : Splattershot
    {
        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 36;
            Item.useTime = 6;
            Item.useAnimation = Item.useTime;
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
