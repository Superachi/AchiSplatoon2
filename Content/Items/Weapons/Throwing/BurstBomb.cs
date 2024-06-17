using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    internal class BurstBomb : BaseBomb
    {
        public override int ExplosionRadius { get => 180; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.shoot = ModContent.ProjectileType<BurstBombProjectile>();
            Item.damage = 30;
            Item.knockBack = 4;
            Item.useTime = 20;
            Item.useAnimation = Item.useTime;
            Item.width = 24;
            Item.height = 28;
        }
    }
}
