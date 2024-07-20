using AchiSplatoon2.Content.Projectiles.SplatanaProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class BaseSplatana : BaseWeapon
    {
        public override string ShootSample { get => "Splatana/WiperStrongSlash"; }
        public override string ShootWeakSample { get => "Splatana/WiperWeakSlash"; }
        public virtual string ChargeSample { get => "Splatana/WiperCharge"; }

        // Splatana specific
        public virtual float[] ChargeTimeThresholds { get => [18f]; }
        public virtual float WeakSlashShotSpeed { get => 8f; }
        public virtual float MaxChargeMeleeDamageMod { get => 5f; }
        public virtual float MaxChargeRangeDamageMod { get => 2f; }
        public virtual float MaxChargeLifetimeMod { get => 3f; }
        public virtual float MaxChargeVelocityMod { get => 0.7f; }
        public override Vector2? HoldoutOffset() { return new Vector2(-8, 8); }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DamageType = DamageClass.Melee;
            Item.damage = 5;
            Item.knockBack = 1;

            Item.width = 62;
            Item.height = 52;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SplatanaChargeProjectile>();

            Item.noMelee = true;
            Item.channel = true;
        }
    }
}
