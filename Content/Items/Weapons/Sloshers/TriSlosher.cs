using AchiSplatoon2.Content.Projectiles.SlosherProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class TriSlosher : Slosher
    {
        public override float InkCost { get => 6f; }
        public override int ShotCount => 8;
        public override float AimDeviation => 5f;
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
            Item.crit = 5;
            Item.knockBack = 5;

            Item.SetValuePostEvilBosses();
        }

        public override void AddRecipes() => AddRecipePostBee();
    }
}
