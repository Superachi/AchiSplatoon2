using AchiSplatoon2.Content.Projectiles.BrellaProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class SplatBrella : BaseBrella
    {
        public override int ProjectileCount { get => 4; }
        public override int ShieldLife => 200;
        public override int ShieldCooldown => 450;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BrellaShotgunProjectile>(),
                singleShotTime: 36,
                shotVelocity: 8f);

            Item.damage = 14;
            Item.width = 50;
            Item.height = 58;
            Item.knockBack = 2;

            Item.SetValuePostEvilBosses();
        }

        public override void AddRecipes() => AddRecipeMeteorite();
    }
}
