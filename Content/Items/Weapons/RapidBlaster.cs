using AchiSplatoon2.Content.Items.Weapons;
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
	public class RapidBlaster : Blaster
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 32;
            Item.knockBack = 4f;
            Item.width = 58;
			Item.height = 38;
			Item.useTime = 35;
            Item.useAnimation = Item.useTime;
			Item.shootSpeed = 14;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteoriteBar, 12);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override Vector2? HoldoutOffset()
        {
			return new Vector2(-8, 4);
        }
    }
}
