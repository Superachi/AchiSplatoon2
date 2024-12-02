using AchiSplatoon2.Content.EnumsAndConstants;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    internal class BambooMk1Charger : BaseCharger
    {
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
