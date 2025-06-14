using AchiSplatoon2.Content.Projectiles.SlosherProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class DreadWringer : BaseSlosher
    {
        public override float InkCost { get => 9f; }
        public override float AimDeviation => 3f;
        public override float ShotGravity { get => 0.1f; }
        public override int ShotCount => 10;
        public override int Repetitions => 1;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SlosherMainProjectile>(),
                singleShotTime: 50,
                shotVelocity: 7.5f
            );
            Item.useStyle = ItemUseStyleID.DrinkLiquid;

            Item.damage = 54;
            Item.width = 46;
            Item.height = 30;
            Item.knockBack = 6;
            Item.SetValuePostMech();
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofFright);
    }
}
