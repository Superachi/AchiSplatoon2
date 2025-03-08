using AchiSplatoon2.Content.Projectiles.SlosherProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class TriSlosher : Slosher
    {
        public override int ShotCount => 8;
        public override float AimDeviation => 6f;
        public override float ShotGravity { get => 0.12f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SlosherMainProjectile>(),
                singleShotTime: 24,
                shotVelocity: 6.5f
            );
            Item.useStyle = ItemUseStyleID.DrinkLiquid;

            Item.damage = 24;
            Item.crit = 5;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes() => AddRecipePostBee();
    }
}
