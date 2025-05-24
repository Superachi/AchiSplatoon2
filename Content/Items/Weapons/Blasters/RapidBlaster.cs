using AchiSplatoon2.Content.Projectiles.BlasterProjectiles;
using AchiSplatoon2.ExtensionMethods;
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
        public override float InkCost { get => 6.5f; }

        public override int ExplosionRadiusAir { get => 160; }
        public override float ExplosionDelayInit { get => 15f; }

        public override Vector2? HoldoutOffset() { return new Vector2(-8, 4); }

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BlasterProjectile>(),
                singleShotTime: 35,
                shotVelocity: 11f);

            Item.damage = 22;
            Item.knockBack = 3;
            Item.width = 58;
            Item.height = 38;
            Item.SetValuePreEvilBosses();
        }

        public override void AddRecipes() => AddRecipePostEOC();
    }
}
