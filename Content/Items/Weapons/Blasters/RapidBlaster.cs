using AchiSplatoon2.Content.Projectiles.BlasterProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    internal class RapidBlaster : Blaster
    {
        public override int ExplosionRadiusAir { get => 160; }
        public override int ExplosionRadiusTile { get => 120; }
        public override float ExplosionDelayInit { get => 15f; }

        public override Vector2? HoldoutOffset() { return new Vector2(-8, 4); }

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BlasterProjectileV2>(),
                singleShotTime: 35,
                shotVelocity: 11f);

            Item.damage = 24;
            Item.knockBack = 3;
            Item.width = 58;
            Item.height = 38;
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes() => base.AddRecipes();
    }
}
