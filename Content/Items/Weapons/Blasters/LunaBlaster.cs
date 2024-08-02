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
    internal class LunaBlaster : Blaster
    {
        // Explosion radius and delay
        public override int ExplosionRadiusAir { get => 280; }
        public override int ExplosionRadiusTile { get => 150; }
        public override float ExplosionDelayInit { get => 10f; }

        public override Vector2? HoldoutOffset() { return new Vector2(-10, -10); }
        public override float MuzzleOffsetPx { get; set; } = 46f;

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BlasterProjectileV2>(),
                singleShotTime: 40,
                shotVelocity: 6f);

            Item.damage = 140;
            Item.width = 42;
            Item.height = 44;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipeOrichalcum();
    }
}
