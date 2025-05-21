using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Projectiles.BlasterProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    [OrderWeapon]
    internal class OrderBlaster : BaseBlaster
    {
        public override int ExplosionRadiusAir { get => 160; }
        public override float ExplosionDelayInit { get => 10f; }

        public override Vector2? HoldoutOffset() { return new Vector2(-10, -10); }
        public override Vector2 MuzzleOffset => new Vector2(46f, 0);

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BlasterProjectile>(),
                singleShotTime: 58,
                shotVelocity: 6f);

            Item.damage = 24;
            Item.width = 42;
            Item.height = 44;
            Item.knockBack = 3;

            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes() => AddRecipeOrder(ItemID.Topaz);
    }
}
