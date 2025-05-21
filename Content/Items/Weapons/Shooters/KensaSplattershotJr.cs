using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class KensaSplattershotJr : SplattershotJr
    {
        public override float InkTankCapacityBonus { get => 60f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SplattershotProjectile>(),
                singleShotTime: 5,
                shotVelocity: 5f);

            Item.damage = 36;
            Item.knockBack = 4f;
            Item.SetValuePostPlantera();
        }
        public override void AddRecipes() => AddRecipeKensa();
    }
}
