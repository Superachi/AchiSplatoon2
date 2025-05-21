using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    internal class EliterCharger : SplatCharger
    {
        public override float InkCost { get => 2.4f; }

        public override SoundStyle ShootSample { get => SoundPaths.EliterChargerShoot.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.EliterChargerShootWeak.ToSoundStyle(); }
        public override float[] ChargeTimeThresholds { get => [75f]; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 92;
            Item.height = 32;
            Item.damage = 85;
            Item.knockBack = 6;
            Item.SetValueLatePreHardmode();
        }

        public override void AddRecipes() => AddRecipePostSkeletron();

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 2);
        }
    }
}
