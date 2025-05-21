using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.Audio;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    internal class ClassicSquiffer : SplatCharger
    {
        public override float InkCost { get => 1.5f; }

        public override SoundStyle ShootSample { get => SoundPaths.SquifferChargerShoot.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.SquifferChargerShootWeak.ToSoundStyle(); }
        public override bool ScreenShake => false;
        public override float[] ChargeTimeThresholds { get => [42f]; }
        public override float RangeModifier => 0.4f;
        public override float MinPartialRange { get => 0.3f; }
        public override float MaxPartialRange { get => 0.6f; }
        public override bool SlowAerialCharge { get => false; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.damage = 50;
            Item.width = 90;
            Item.height = 26;
            Item.damage = 48;

            Item.SetValuePostEvilBosses();
        }

        public override void AddRecipes() => AddRecipePostBee();
    }
}
