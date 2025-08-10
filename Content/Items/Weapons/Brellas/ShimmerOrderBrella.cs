using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Projectiles.BrellaProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    [ShimmerOrderWeapon]
    internal class ShimmerOrderBrella : OrderBrella
    {
        public override float InkCost { get => 0f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BrellaShotgunProjectile>(),
                singleShotTime: 36,
                shotVelocity: 8f);

            Item.SetValuePostEvilBosses();
        }

        public override void AddRecipes() { }
    }
}
