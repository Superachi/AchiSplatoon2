using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class KensaSplatDualie : EnperrySplatDualie
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<DualieShotProjectile>(),
                singleShotTime: 5,
                shotVelocity: 5f);

            Item.damage = 36;
            Item.width = 50;
            Item.height = 36;
            Item.knockBack = 4;
            Item.SetValuePostPlantera();
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
