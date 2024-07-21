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
    internal class RangeBlaster : Blaster
    {
        // Explosion radius and delay
        public override int ExplosionRadiusAir { get => 240; }
        public override int ExplosionRadiusTile { get => 160; }
        public override float ExplosionDelayInit { get => 15f; }

        public override Vector2? HoldoutOffset() { return new Vector2(-12, -2); }
        public override float MuzzleOffsetPx { get; set; } = 70f;

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BlasterProjectileV2>(),
                singleShotTime: 58,
                shotVelocity: 9f);

            Item.damage = 260;
            Item.width = 64;
            Item.height = 34;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.TitaniumBar, 5);
            recipe.Register();
        }
    }
}
