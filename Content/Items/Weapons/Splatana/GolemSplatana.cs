using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.SplatanaProjectiles.GolemSplatanaProjectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class GolemSplatana : BaseSplatana
    {
        public override float InkCost { get => 1.5f; }

        public override SoundStyle ShootSample { get => SoundPaths.Silence.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundID.DD2_BetsyFireballShot; }
        public override SoundStyle ChargeSample { get => SoundID.DD2_DarkMageAttack; }

        // Splatana specific
        public override int BaseDamage { get => 80; }
        public override float[] ChargeTimeThresholds { get => [36f]; }
        public override float MaxChargeMeleeDamageMod { get => 3f; }
        public override float MaxChargeRangeDamageMod { get => 2f; }

        public override int MeleeEnergyProjectile { get => ModContent.ProjectileType<GolemSplatanaMeleeEnergyProjectile>(); }
        public override bool EnableWeakSlashProjectile => false;
        public override bool EnableStrongSlashProjectile => false;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = DisplayDamage(BaseDamage);
            Item.knockBack = 5;

            Item.useTime = 30;
            Item.useAnimation = Item.useTime;

            Item.width = 64;
            Item.height = 64;

            Item.value = Item.buyPrice(gold: 50);
            Item.rare = ItemRarityID.Yellow;
        }
    }
}
