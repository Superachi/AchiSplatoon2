using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AchiSplatoon2.Content.Projectiles.RollerProjectiles;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Content.Projectiles.BrushProjectiles;
using AchiSplatoon2.Content.Projectiles.SplatanaProjectiles;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class BaseRoller : BaseWeapon
    {
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Roller;
        public virtual string WindUpSample { get => "Rollers/SwingMedium"; }
        public virtual string SwingSample { get => "Rollers/Fling1"; }

        public virtual float ShotGravity { get => 0.1f; }
        public virtual int ShotGravityDelay { get => 0; }
        public virtual int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 6f; }
        public override Vector2? HoldoutOffset() { return new Vector2(4, 0); }
        public override float MuzzleOffsetPx { get; set; } = 44f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DamageType = DamageClass.Melee;
            Item.width = 56;
            Item.height = 50;
            Item.damage = 85;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<RollerSwingProjectile>();

            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<RollerSwingProjectile>()] == 0;
        }
    }
}
