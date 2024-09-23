using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    internal class InkMine : BaseBomb
    {
        public override int ExplosionRadius { get => 250; }
        public int DetectionRadius { get => 160; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.shoot = ModContent.ProjectileType<InkMineProjectile>();
            Item.damage = 120;
            Item.knockBack = 9;
            Item.useTime = 30;
            Item.useAnimation = Item.useTime;
            Item.width = 32;
            Item.height = 22;
        }
    }
}
