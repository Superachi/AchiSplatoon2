using AchiSplatoon2.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items
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
			Item.damage = 42;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(gold: 5);
			Item.rare = ItemRarityID.LightRed;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PinkGel, 5);
            recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

        public override Vector2? HoldoutOffset()
        {
			return new Vector2(-4, 2);
        }
    }
}
