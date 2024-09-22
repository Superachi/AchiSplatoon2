using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class SplooshOMatic : BaseSplattershot
    {

        public override string ShootSample => "SplatlingShoot";
        public override float MuzzleOffsetPx { get; set; } = 62f;
        public override Vector2? HoldoutOffset() { return new Vector2(-10, 0); }
        public override float ShotGravity { get => 0.5f; }
        public override int ShotGravityDelay => 4;
        public override int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 10f; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SplattershotProjectile>(),
                singleShotTime: 5,
                shotVelocity: 3.5f);

            Item.damage = 22;
            Item.width = 60;
            Item.height = 32;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes() => AddRecipeWithSheldonLicenseSilver();
    }
}
