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

            Item.damage = 56;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes() => AddRecipeCobalt();
    }
}
