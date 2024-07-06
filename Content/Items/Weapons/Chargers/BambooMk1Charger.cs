using AchiSplatoon2.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    internal class BambooMk1Charger : BaseCharger
    {
        public override string ShootSample { get => "BambooChargerShoot"; }
        public override string ShootWeakSample { get => "BambooChargerShootWeak"; }
        public override Vector2? HoldoutOffset() { return new Vector2(-18, 0); }
        public override float MuzzleOffsetPx { get; set; } = 48f;
        public override float[] ChargeTimeThresholds { get => [20f]; }
        public override bool ScreenShake => false;
        public override int MaxPenetrate => 1;
        public override bool DirectHitEffect => false;

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SplatChargerProjectile>(),
                singleShotTime: 12,
                shotVelocity: 12f);

            Item.width = 74;
            Item.height = 24;
            Item.damage = 40;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            AddRecipeWithSheldonLicenseBasic();

            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Lens, 6);
            recipe.AddIngredient(ItemID.JungleSpores, 10);
            recipe.AddIngredient(ItemID.BambooBlock, 30);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
