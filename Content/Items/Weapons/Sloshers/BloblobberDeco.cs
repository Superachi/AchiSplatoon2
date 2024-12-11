using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class BloblobberDeco : Bloblobber
    {
        public override int BurstShotCount { get => 6; }
        public override int BurstShotDelay { get => 3; }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 58;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes()
        {
        }
    }
}
