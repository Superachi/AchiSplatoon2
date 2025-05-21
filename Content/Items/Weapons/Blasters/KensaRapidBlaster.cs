using AchiSplatoon2.Content.Projectiles.BlasterProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    internal class KensaRapidBlaster : RapidBlaster
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BlasterProjectile>(),
                singleShotTime: 25,
                shotVelocity: 11f);

            Item.damage = 110;
            Item.SetValuePostPlantera();
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
