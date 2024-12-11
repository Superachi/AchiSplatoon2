using AchiSplatoon2.Content.Projectiles.BrellaProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class UndercoverBrella : BaseBrella
    {
        public override float AimDeviation { get => 4f; }

        // Brella specific
        public override int ProjectileCount { get => 4; }
        public override float ShotgunArc { get => 2f; }
        public override int ShieldLife => 200;
        public override int ShieldCooldown => 360;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BrellaShotgunProjectile>(),
                singleShotTime: 24,
                shotVelocity: 8f);

            Item.damage = 22;
            Item.width = 50;
            Item.height = 58;
            Item.knockBack = 2;

            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes() => AddRecipeAdamantite();
    }
}
