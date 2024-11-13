using AchiSplatoon2.Content.Projectiles.SplatanaProjectiles;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class BaseSplatana : BaseWeapon
    {
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Splatana;

        public override string ShootSample { get => "Splatana/WiperStrongSlash"; }
        public override string ShootWeakSample { get => "Splatana/WiperWeakSlash"; }
        public virtual string ChargeSample { get => "Splatana/WiperCharge"; }
        public override bool SlowAerialCharge { get => false; }

        // Splatana specific
        public virtual int BaseDamage { get => 5; }
        public virtual float[] ChargeTimeThresholds { get => [18f]; }
        public virtual float WeakSlashShotSpeed { get => 8f; }
        public virtual float MaxChargeMeleeDamageMod { get => 3f; }
        public virtual float MaxChargeRangeDamageMod { get => 2f; }
        public virtual float MaxChargeLifetimeMod { get => 3f; }
        public virtual float MaxChargeVelocityMod { get => 0.7f; }
        public override Vector2? HoldoutOffset() { return new Vector2(-8, 8); }

        public virtual int WeakSlashProjectile { get => ModContent.ProjectileType<SplatanaWeakSlashProjectile>(); }
        public virtual int StrongSlashProjectile { get => ModContent.ProjectileType<SplatanaStrongSlashProjectile>(); }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DamageType = DamageClass.Melee;

            // Apply the max charge range damage mod to give the player a better indication of the damage
            Item.damage = DisplayDamage(BaseDamage);
            Item.knockBack = 1;

            Item.width = 62;
            Item.height = 52;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SplatanaChargeProjectile>();

            Item.noMelee = true;
            Item.channel = true;
        }

        public int DisplayDamage(int damage)
        {
            return (int)(damage * MaxChargeRangeDamageMod);
        }

        public int ActualDamage(int damage)
        {
            return (int)(damage / MaxChargeRangeDamageMod);
        }

        public override int ChoosePrefix(UnifiedRandom rand)
        {
            return rand.NextFromCollection<int>(PrefixHelper.ListChargeWeaponsPrefixes());
        }
    }
}
