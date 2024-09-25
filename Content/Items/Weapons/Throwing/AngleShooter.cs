using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    internal class AngleShooter : BaseBomb
    {
        public override int MaxBounces { get => 5; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.shoot = ModContent.ProjectileType<AngleShooterProjectile>();
            Item.shootSpeed = 1f;
            Item.damage = 25;
            Item.knockBack = 6;
            Item.width = 30;
            Item.height = 16;
        }
    }
}
