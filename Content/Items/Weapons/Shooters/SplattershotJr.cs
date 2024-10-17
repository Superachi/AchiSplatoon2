using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class SplattershotJr : BaseSplattershot
    {
        public override float ShotGravity { get => 0.4f; }
        public override int ShotGravityDelay { get => 4; }
        public override int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 8f; }
        public override string ShootSample { get => "SplattershotShoot"; }
        public override Vector2? HoldoutOffset() { return new Vector2(-4, 0); }
        public override float MuzzleOffsetPx { get; set; } = 44f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SplattershotProjectile>(),
                singleShotTime: 6,
                shotVelocity: 4f);

            Item.damage = 9;
            Item.width = 48;
            Item.height = 30;
            Item.knockBack = 0.5f;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;

            Item.ArmorPenetration = 3;
        }

        public override void AddRecipes() => AddRecipeMeteorite();
    }
}
