using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.SplatanaProjectiles.EelSplatanaProjectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class EelSplatana : BaseSplatana
    {
        public override float InkCost { get => 5f; }
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Splatana;

        public override SoundStyle ShootSample { get => SoundPaths.Silence.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.SplatanaWiperWeakSlash.ToSoundStyle(); }
        public override SoundStyle ChargeSample { get => SoundPaths.SplatanaStamperCharge.ToSoundStyle(); }

        // Splatana specific
        public override int BaseDamage { get => 80; }
        public override float[] ChargeTimeThresholds { get => [60f]; }
        public override float WeakSlashShotSpeed { get => 8f; }
        public override float MaxChargeRangeDamageMod { get => 1f; }
        public override float MaxChargeLifetimeMod { get => 15f; }
        public override float MaxChargeVelocityMod { get => 0.9f; }

        public override int StrongSlashProjectile { get => ModContent.ProjectileType<EelSplatanaStrongSlashProjectile>(); }


        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = DisplayDamage(BaseDamage);
            Item.knockBack = 5;

            Item.useTime = 22;
            Item.useAnimation = Item.useTime;

            Item.width = 58;
            Item.height = 58;

            Item.value = Item.buyPrice(gold: 50);
            Item.rare = ItemRarityID.Yellow;
        }
    }
}
