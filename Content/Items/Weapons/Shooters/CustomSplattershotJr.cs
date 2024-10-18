using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class CustomSplattershotJr : SplattershotJr
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SplattershotProjectile>(),
                singleShotTime: 5,
                shotVelocity: 4.5f);

            Item.damage = 20;
            Item.width = 48;
            Item.height = 30;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes() => AddRecipeWithSheldonLicenseSilver();
    }
}
