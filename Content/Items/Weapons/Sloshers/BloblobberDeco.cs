using AchiSplatoon2.Content.Projectiles.SlosherProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class BloblobberDeco : Bloblobber
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 64;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes() => AddRecipePostPlanteraDungeon();
    }
}
