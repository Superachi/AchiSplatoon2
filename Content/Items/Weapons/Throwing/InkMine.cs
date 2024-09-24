using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    internal class InkMine : BaseBomb
    {
        public override int ExplosionRadius { get => 200; }
        public int DetectionRadius { get => 120; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.shoot = ModContent.ProjectileType<InkMineProjectile>();
            Item.damage = 90;
            Item.knockBack = 9;
            Item.width = 32;
            Item.height = 22;
        }
    }
}
