using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    internal class SplatBomb : BaseBomb
    {
        public override int ExplosionRadius { get => 250; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.shoot = ModContent.ProjectileType<SplatBombProjectile>();
            Item.damage = 60;
            Item.knockBack = 8;
            Item.width = 28;
            Item.height = 28;
        }
    }
}
