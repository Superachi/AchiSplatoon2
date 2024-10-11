using AchiSplatoon2.Content.Projectiles.BrushProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class BaseBrush : BaseWeapon
    {
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Brush;

        public override float AimDeviation { get => 12f; }
        public override string ShootSample { get => "BrushShoot"; }
        public override string ShootAltSample { get => "BrushShootAlt"; }
        protected virtual int ArmorPierce => 0;
        public virtual float DelayUntilFall => 10f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ArmorPierce);

        // Brush-specific properties
        public virtual float ShotVelocity => 7f;
        public virtual float ShotGravity => 0.2f;
        public virtual float BaseWeaponUseTime => 15f;
        public virtual int SwingArc => 80;

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.DamageType = DamageClass.Melee;
            Item.ArmorPenetration = ArmorPierce;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BrushSwingProjectile>();
            Item.shootSpeed = 6f;

            Item.useTime = 6;
            Item.useAnimation = Item.useTime;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] == 0;
        }

        protected void SetItemUseTime()
        {
            Item.useTime = (int)BaseWeaponUseTime;
            Item.useAnimation = Item.useTime;
        }
    }
}
