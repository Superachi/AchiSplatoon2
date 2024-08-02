using AchiSplatoon2.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class SplashOMatic : SplooshOMatic
    {
        public override float ShotGravity { get => 0.4f; }
        public override int ShotGravityDelay => 10;
        public override int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 0f; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SplattershotProjectile>(),
                singleShotTime: 5,
                shotVelocity: 6f);

            Item.damage = 18;
            Item.width = 60;
            Item.height = 32;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes() => AddRecipeWithSheldonLicenseSilver();
    }
}
