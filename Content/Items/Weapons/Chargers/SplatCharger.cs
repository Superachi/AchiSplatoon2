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
    internal class SplatCharger : BaseCharger
    {
        public override string ShootSample { get => "SplatChargerShoot"; }
        public override string ShootWeakSample { get => "SplatChargerShootWeak"; }
        public override Vector2? HoldoutOffset() { return new Vector2(-20, 2); }
        public override float MuzzleOffsetPx { get; set; } = 60f;
        public override float[] ChargeTimeThresholds { get => [55f]; }

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SplatChargerProjectile>(),
                singleShotTime: 15,
                shotVelocity: 12f);
            Item.damage = 66;
            Item.width = 82;
            Item.height = 26;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes() => AddRecipePostEOC();
    }
}
