using AchiSplatoon2.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Projectiles.StringerProjectiles;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    public class TriStringer : BaseWeapon
    {
        public override Vector2? HoldoutOffset() { return new Vector2(0, 2); }
        public override float MuzzleOffsetPx { get; set; } = 50f;

        public override void SetDefaults()
        {
            Item.DefaultToRangedWeapon(ModContent.ProjectileType<TriStringerCharge>(), AmmoID.None, 15, 12f);
            Item.damage = 32;
            Item.width = 34;
            Item.height = 74;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Green;
            Item.noMelee = true;
            Item.channel = true;
            Item.crit = 8;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DemonBow, 1);
            recipe.AddIngredient(ItemID.ReinforcedFishingPole, 1);
            recipe.AddIngredient(ItemID.SharkFin, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe altRecipe = CreateRecipe();
            altRecipe.AddIngredient(ItemID.TendonBow, 1);
            altRecipe.AddIngredient(ItemID.ReinforcedFishingPole, 1);
            altRecipe.AddIngredient(ItemID.SharkFin, 1);
            altRecipe.AddTile(TileID.Anvils);
            altRecipe.Register();
        }
    }
}
