using AchiSplatoon2.Content.Projectiles.BrushProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class BaseBrush : BaseWeapon
    {
        public override float AimDeviation { get => 16f; }
        public override string ShootSample { get => "BrushShoot"; }
        public override string ShootAltSample { get => "BrushShootAlt"; }
        protected virtual int ArmorPierce => 10;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ArmorPierce);

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DamageType = DamageClass.Melee;
            Item.ArmorPenetration = ArmorPierce;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<InkbrushProjectile>();
            Item.shootSpeed = 8f;
        }
    }
}
