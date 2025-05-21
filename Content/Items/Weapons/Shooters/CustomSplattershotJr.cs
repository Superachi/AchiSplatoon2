using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class CustomSplattershotJr : SplattershotJr
    {
        public override float InkTankCapacityBonus { get => 40f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SplattershotProjectile>(),
                singleShotTime: 5,
                shotVelocity: 4.5f);

            Item.damage = 12;
            Item.width = 48;
            Item.height = 30;
            Item.knockBack = 3f;

            Item.SetValueLowHardmodeOre();
        }

        public override void AddRecipes() => AddRecipeWithSheldonLicenseSilver();
    }
}
