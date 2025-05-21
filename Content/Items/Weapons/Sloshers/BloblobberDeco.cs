using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class BloblobberDeco : Bloblobber
    {
        public override int BurstShotCount { get => 5; }
        public override int BurstShotDelay { get => 4; }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 58;
            Item.SetValuePostPlantera();
        }

        public override void AddRecipes()
        {
        }
    }
}
