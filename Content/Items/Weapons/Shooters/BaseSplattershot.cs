using AchiSplatoon2.Content.Projectiles;
using Microsoft.Xna.Framework;
using System.Reflection.Metadata;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class BaseSplattershot : BaseWeapon
    {
        public virtual float ShotGravity { get => 0.1f; }
        public virtual int ShotGravityDelay { get => 0; }
        public virtual int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 6f; }
        public override string ShootSample { get => "SplattershotShoot"; }
        public override Vector2? HoldoutOffset() { return new Vector2(4, 0); }
        public override float MuzzleOffsetPx { get; set; } = 44f;

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToRangedWeapon(
                baseProjType: ModContent.ProjectileType<SplattershotProjectile>(),
                ammoID: AmmoID.None,
                singleShotTime: 8,
                shotVelocity: 6f);

            Item.useAnimation = Item.useTime;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.noMelee = true;
        }
    }
}
