using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Buffs.Debuffs;
using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    [ItemCategory("Sub weapon", "Throwing")]
    internal class PointSensor : BaseBomb
    {
        public override float InkCost { get => 40f; }
        public override int ExplosionRadius { get => 220; }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(100 / MarkedBuff.CritChanceDenominator);

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
