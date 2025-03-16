using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.ChargerProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    internal class BambooMk1Charger : BaseCharger
    {
        /// <summary>
        /// When the target has this percentage of life left, Bamboozler shots will deal bonus damage.
        /// </summary>
        public static float LowLifeTapShotThreshold => 0.3f;
        protected override string UsageHintParamA => $"{(int)(LowLifeTapShotThreshold * 100)}";
        public static float LowLifeTapShotDamageMult => 3f;
        public static float LowLifeTapShotDamageBossMult => 2f;

        public override float InkCost { get => 1f; }

        public override SoundStyle ShootSample { get => SoundPaths.BambooChargerShoot.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.BambooChargerShootWeak.ToSoundStyle(); }
        public override Vector2? HoldoutOffset() { return new Vector2(-18, 0); }
        public override float MuzzleOffsetPx { get; set; } = 48f;
        public override float[] ChargeTimeThresholds { get => [18f]; }
        public override float MinPartialRange { get => 1f; }
        public override float MaxPartialRange { get => 1f; }
        public override bool ScreenShake => false;
        public override int MaxPenetrate => 1;
        public override bool DirectHitEffect => false;
        public override int ProjectileType { get => ModContent.ProjectileType<BambooChargerProjectile>(); }


        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();
            Item.width = 74;
            Item.height = 24;
            Item.damage = 20;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            AddRecipeWithSheldonLicenseBasic();

            CreateRecipe()
                .AddTile(TileID.Anvils)
                .AddIngredient(ItemID.Lens, 6)
                .AddIngredient(ItemID.JungleSpores, 10)
                .AddIngredient(ItemID.BambooBlock, 30)
                .Register();
        }
    }
}
