using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    [ItemCategory("Sub weapon", "Throwing")]
    internal class PointSensor : BaseBomb
    {
        public override float InkCost { get => 40f; }
        public override int ExplosionRadius { get => 220; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.shoot = ModContent.ProjectileType<PointSensorProjectile>();
            Item.damage = 0;
            Item.useAnimation = Item.useTime;
            Item.width = 28;
            Item.height = 22;
        }
    }
}
