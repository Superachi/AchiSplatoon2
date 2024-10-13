using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class IceStringer : BaseStringer
    {
        public override float[] ChargeTimeThresholds { get => [20f, 40f]; }
        public override string ShootSample { get => "BambooChargerShoot"; }
        public override string ShootWeakSample { get => "BambooChargerShootWeak"; }
        public override float ShotgunArc { get => 0f; }
        public override int ProjectileCount { get => 1; }
        public override bool AllowStickyProjectiles { get => true; }

        public override Vector2? HoldoutOffset() { return new Vector2(0, 2); }
        public override float MuzzleOffsetPx { get; set; } = 50f;

        public override int ProjectileType => ModContent.ProjectileType<IceStringerProjectile>();

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 190;
            Item.width = 36;
            Item.height = 82;
            Item.knockBack = 7;

            Item.value = Item.buyPrice(gold: 50);
            Item.rare = ItemRarityID.Yellow;
        }
    }
}
