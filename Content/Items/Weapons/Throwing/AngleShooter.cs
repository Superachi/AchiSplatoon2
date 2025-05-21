using AchiSplatoon2.Content.Buffs.Debuffs;
using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    internal class AngleShooter : BaseBomb
    {
        public override float InkCost { get => 30f; }
        public override int MaxBounces { get => 5; }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(100 / MarkedBuff.CritChanceDenominator);

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.shoot = ModContent.ProjectileType<AngleShooterProjectile>();
            Item.shootSpeed = 1f;
            Item.damage = 40;
            Item.knockBack = 6;
            Item.width = 30;
            Item.height = 16;
        }
    }
}
