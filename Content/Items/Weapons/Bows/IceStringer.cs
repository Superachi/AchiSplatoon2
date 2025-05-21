using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class IceStringer : BaseStringer
    {
        public override float InkCost { get => 2f; }

        public override float[] ChargeTimeThresholds { get => [20f, 40f]; }
        public override SoundStyle ShootSample { get => SoundPaths.BambooChargerShoot.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.BambooChargerShootWeak.ToSoundStyle(); }
        public override float ShotgunArc { get => 0f; }
        public override int ProjectileCount { get => 1; }
        public override bool AllowStickyProjectiles { get => true; }

        public override Vector2? HoldoutOffset() { return new Vector2(0, 2); }
        public override Vector2 MuzzleOffset => new Vector2(50f, 0);

        public override int ProjectileType => ModContent.ProjectileType<IceStringerProjectile>();

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 200;
            Item.width = 36;
            Item.height = 82;
            Item.knockBack = 8;

            Item.SetValueEndgame();
        }
    }
}
