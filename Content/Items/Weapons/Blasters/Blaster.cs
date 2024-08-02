using AchiSplatoon2.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    internal class Blaster : BaseBlaster
    {
        // Explosion radius and delay
        public override int ExplosionRadiusAir { get => 240; }
        public override int ExplosionRadiusTile { get => 160; }
        public override float ExplosionDelayInit { get => 12f; }

        // Sprite offset
        public override Vector2? HoldoutOffset() { return new Vector2(-12, -2); }
        public override float MuzzleOffsetPx { get; set; } = 64f;

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BlasterProjectileV2>(),
                singleShotTime: 50,
                shotVelocity: 7f);

            Item.damage = 46;
            Item.width = 64;
            Item.height = 34;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes() => AddRecipeMeteorite();
    }
}
