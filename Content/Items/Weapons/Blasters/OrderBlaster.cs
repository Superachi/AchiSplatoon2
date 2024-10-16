using AchiSplatoon2.Content.Projectiles.BlasterProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    internal class OrderBlaster : BaseBlaster
    {
        public override int ExplosionRadiusAir { get => 160; }
        public override int ExplosionRadiusTile { get => 120; }
        public override float ExplosionDelayInit { get => 10f; }

        public override Vector2? HoldoutOffset() { return new Vector2(-10, -10); }
        public override float MuzzleOffsetPx { get; set; } = 46f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BlasterProjectileV2>(),
                singleShotTime: 58,
                shotVelocity: 6f);

            Item.damage = 30;
            Item.width = 42;
            Item.height = 44;
            Item.knockBack = 2;

            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes() => AddRecipeOrder();
    }
}
