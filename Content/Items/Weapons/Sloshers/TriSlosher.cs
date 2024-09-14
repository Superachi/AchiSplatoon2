using Terraria.ID;
using Terraria;
using AchiSplatoon2.Content.Projectiles.SlosherProjectiles;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class TriSlosher : Slosher
    {
        public override float ShotGravity { get => 0.15f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SlosherMainProjectile>(),
                singleShotTime: 24,
                shotVelocity: 6f
            );
            Item.useStyle = ItemUseStyleID.DrinkLiquid;

            Item.damage = 22;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes() => AddRecipeHellstone();
    }
}
