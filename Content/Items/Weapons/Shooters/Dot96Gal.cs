using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class Dot96Gal : Dot52Gal
    {
        public override int DamageOverride => 96;
        public override string ShootSample { get => "Dot52GalShoot"; }
        public override float MuzzleOffsetPx { get; set; } = 48f;
        public override Vector2? HoldoutOffset() { return new Vector2(-2, 0); }

        public override float ShotGravity { get => 0.5f; }
        public override int ShotGravityDelay => 16;
        public override float AimDeviation { get => 4f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<Dot52GalProjectile>(),
                singleShotTime: 12,
                shotVelocity: 9f);

            Item.damage = 96;
            Item.width = 62;
            Item.height = 32;
            Item.knockBack = 8f;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipeChlorophyteUpgrade(true, ModContent.ItemType<Dot52Gal>());
    }
}
