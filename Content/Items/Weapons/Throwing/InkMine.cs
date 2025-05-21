using AchiSplatoon2.Content.Buffs.Debuffs;
using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    internal class InkMine : BaseBomb
    {
        public override int ExplosionRadius { get => 200; }
        public int DetectionRadius { get => 120; }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(100 / MarkedBuff.CritChanceDenominator);

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.shoot = ModContent.ProjectileType<InkMineProjectile>();
            Item.damage = 50;
            Item.knockBack = 9;
            Item.width = 32;
            Item.height = 22;
        }
    }
}
