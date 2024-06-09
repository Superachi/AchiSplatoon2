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
    public class RapidBlasterPro : RapidBlaster
    {
        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 100;
            Item.width = 72;
            Item.height = 42;
            Item.useTime = 40;
            Item.useAnimation = Item.useTime;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
            Item.shootSpeed = 18;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RapidBlaster>());
            recipe.AddIngredient(ItemID.AdamantiteBar, 10);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe altRecipe = CreateRecipe();
            altRecipe.AddIngredient(ModContent.ItemType<RapidBlaster>());
            altRecipe.AddIngredient(ItemID.TitaniumBar, 10);
            altRecipe.AddIngredient(ItemID.SoulofLight, 5);
            altRecipe.AddTile(TileID.Anvils);
            altRecipe.Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 4);
        }
    }
}
