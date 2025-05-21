using AchiSplatoon2.Content.Projectiles.ShooterProjectiles.NozzlenoseProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class H3Nozzlenose : L3Nozzlenose
    {
        public override float ShotVelocity { get => 10f; }
        public override int BurstShotTime { get => 5; }
        public override float DamageIncreasePerHit { get => 1.0f; }

        public override int ShotGravityDelay { get => 30; }
        public override float AimDeviation { get => 1f; }
        public override Vector2? HoldoutOffset() { return new Vector2(-2, 0); }
        public override Vector2 MuzzleOffset => new Vector2(56f, -12f);

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<NozzlenoseShooter>(),
                singleShotTime: 26,
                shotVelocity: 1f);

            Item.damage = 32;
            Item.width = 50;
            Item.height = 32;
            Item.knockBack = 2;
            Item.SetValueHighHardmodeOre();
        }

        public override void AddRecipes() => AddRecipeAdamantite();
    }
}
