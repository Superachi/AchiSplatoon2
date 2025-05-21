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
    internal class SplatCharger : BaseCharger
    {
        public override SoundStyle ShootSample { get => SoundPaths.SplatChargerShoot.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.SplatChargerShootWeak.ToSoundStyle(); }
        public override Vector2? HoldoutOffset() { return new Vector2(-20, 2); }
        public override Vector2 MuzzleOffset => new Vector2(60f, 0);
        public override float[] ChargeTimeThresholds { get => [55f]; }

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();
            Item.damage = 52;
            Item.width = 82;
            Item.height = 26;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes() => AddRecipePostEOC();
    }
}
