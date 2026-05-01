using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Projectiles.ChargerProjectiles;
using AchiSplatoon2.ExtensionMethods;
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
    internal class FungalCharger : BaseCharger
    {
        public override float InkCost { get => 2.5f; }
        public static int ProjectileCount => 5;
        public static float ShotgunArc => 5f;
        public override int MaxPenetrate => 1;
        public override bool DirectHitEffect => false;

        public override SoundStyle ShootSample { get => SoundPaths.SnipewriterShoot.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.OrderChargerShootWeak.ToSoundStyle(); }
        public override Vector2? HoldoutOffset() { return new Vector2(-18, -2); }
        public override Vector2 MuzzleOffset => new Vector2(60f, 0);
        public override float[] ChargeTimeThresholds { get => [45f]; }
        public override float RangeModifier => 0.2f;
        public override float MinPartialRange { get => 0.5f; }
        public override float MaxPartialRange { get => 0.8f; }
        public override int ProjectileType { get => ModContent.ProjectileType<FungalChargerProjectile>(); }

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();
            Item.damage = 17;
            Item.width = 82;
            Item.height = 26;
            Item.knockBack = 7;
            Item.SetValuePreEvilBosses();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.Anvils)
                .AddIngredient(ItemID.Lens, 1)
                .AddIngredient(ModContent.ItemType<InkDroplet>(), 3)
                .AddIngredient(ItemID.GlowingMushroom, 30)
                .AddIngredient(ItemID.DemoniteBar, 10)
                .Register();
        }
    }
}