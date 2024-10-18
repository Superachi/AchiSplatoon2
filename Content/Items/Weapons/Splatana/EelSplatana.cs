using AchiSplatoon2.Content.Projectiles.SplatanaProjectiles.EelSplatana;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class EelSplatana : BaseSplatana
    {
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Splatana;

        public override string ShootSample { get => "Splatana/StamperStrongSlash"; }
        public override string ShootWeakSample { get => "Splatana/StamperWeakSlash"; }
        public override string ChargeSample { get => "Splatana/StamperCharge"; }

        // Splatana specific
        public override int BaseDamage { get => 120; }
        public override float[] ChargeTimeThresholds { get => [60f]; }
        public override float WeakSlashShotSpeed { get => 10f; }
        public override float MaxChargeRangeDamageMod { get => 1f; }
        public override float MaxChargeLifetimeMod { get => 15f; }
        public override float MaxChargeVelocityMod { get => 1f; }

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
