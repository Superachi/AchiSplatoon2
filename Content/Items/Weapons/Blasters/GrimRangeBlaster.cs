using AchiSplatoon2.Content.Projectiles.BlasterProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    internal class GrimRangeBlaster : Blaster
    {
        // Explosion radius and delay
        public override int ExplosionRadiusAir { get => 280; }
        public override float ExplosionDelayInit { get => 15f; }

        public override Vector2? HoldoutOffset() { return new Vector2(-12, -2); }
        public override Vector2 MuzzleOffset => new Vector2(70f, 0);

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BlasterProjectile>(),
                singleShotTime: 58,
                shotVelocity: 9f);

            Item.damage = 220;
            Item.width = 68;
            Item.height = 34;
            Item.knockBack = 6;
            Item.SetValuePostPlantera();
        }

        public override void AddRecipes() { }
    }
}
