using AchiSplatoon2.Content.EnumsAndConstants;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Projectiles.SplatanaProjectiles.GolemSplatana;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class GolemSplatanaWeapon : BaseSplatana
    {
        public static int UseTime { get => 30; }
        public override float InkCost { get => 1.5f; }

        public override SoundStyle ShootSample { get => SoundPaths.Silence.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundID.DD2_BetsyFireballShot; }
        public override SoundStyle ChargeSample { get => SoundID.Item34.WithPitchOffset(0); }

        // Splatana specific
        public override int BaseDamage { get => 90; }
        public override float[] ChargeTimeThresholds { get => [30f]; }
        public override float MaxChargeMeleeDamageMod { get => 3f; }
        public override float MaxChargeRangeDamageMod { get => 2f; }
        public override bool EnableWeakSlashProjectile => false;
        public override int StrongSlashProjectile { get => ModContent.ProjectileType<GolemSplatanaStrongSlashProjectile>(); }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = DisplayDamage(BaseDamage);
            Item.knockBack = 8;

            Item.useTime = UseTime;
            Item.useAnimation = Item.useTime;

            Item.width = 64;
            Item.height = 64;

            Item.value = Item.buyPrice(gold: 50);
            Item.rare = ItemRarityID.Yellow;
        }
    }
}
