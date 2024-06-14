using AchiSplatoon2.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    internal class RangeBlaster : Blaster
    {
        // Explosion radius and delay
        public override int ExplosionRadiusAir { get => 240; }
        public override int ExplosionRadiusTile { get => 160; }
        public override float ExplosionDelayInit { get => 20f; }

        public override Vector2? HoldoutOffset() { return new Vector2(-12, -2); }
        public override float MuzzleOffsetPx { get; set; } = 70f;

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToRangedWeapon(
                baseProjType: ModContent.ProjectileType<BlasterProjectile>(),
                ammoID: AmmoID.None,
                singleShotTime: 60,
                shotVelocity: 12f);

            Item.damage = 240;
            Item.width = 64;
            Item.height = 34;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Blaster>());
            recipe.AddIngredient(ItemID.AdamantiteBar, 10);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe altRecipe = CreateRecipe();
            altRecipe.AddIngredient(ModContent.ItemType<Blaster>());
            altRecipe.AddIngredient(ItemID.TitaniumBar, 10);
            altRecipe.AddIngredient(ItemID.SoulofNight, 5);
            altRecipe.AddTile(TileID.Anvils);
            altRecipe.Register();
        }
    }
}
