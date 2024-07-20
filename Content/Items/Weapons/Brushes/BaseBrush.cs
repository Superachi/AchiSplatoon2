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
        public override float AimDeviation { get => 12f; }
        public override string ShootSample { get => "BrushShoot"; }
        public override string ShootAltSample { get => "BrushShootAlt"; }
        protected virtual int ArmorPierce => 0;
        public virtual float DelayUntilFall => 3f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ArmorPierce);

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DamageType = DamageClass.Melee;
            Item.ArmorPenetration = ArmorPierce;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<InkbrushProjectile>();
            Item.shootSpeed = 6f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Occasionally, shoot an extra projectile
            if (Main.rand.NextBool(5))
            {
                CreateProjectileWithWeaponProperties(player, source, velocity + Main.rand.NextVector2Circular(-2, 2));
            }

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}
