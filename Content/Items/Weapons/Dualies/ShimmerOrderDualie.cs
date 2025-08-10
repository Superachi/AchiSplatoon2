using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    [ShimmerOrderWeapon]
    internal class ShimmerOrderDualie : OrderDualie
    {
        public override float InkCost { get => 0f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<DualieShotProjectile>(),
                singleShotTime: 7,
                shotVelocity: 4f);

            Item.SetValuePostEvilBosses();
        }

        public override void AddRecipes() { }
    }
}
