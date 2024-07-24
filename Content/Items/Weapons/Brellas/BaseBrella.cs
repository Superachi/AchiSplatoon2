using AchiSplatoon2.Content.Projectiles.BrellaProjectiles;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class BaseBrella : BaseWeapon
    {
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Brella;

        public virtual float ShotGravity { get => 0.4f; }
        public virtual int ShotGravityDelay { get => 18; }
        public virtual int ShotExtraUpdates { get => 3; }
        public override float AimDeviation { get => 6f; }
        public override string ShootSample { get => "Brellas/BrellaShot"; }
        public override Vector2? HoldoutOffset() { return new Vector2(4, 0); }
        public override float MuzzleOffsetPx { get; set; } = 44f;

        // Brella specific
        public virtual int ProjectileCount { get => 10; }
        public virtual float ShotgunArc { get => 3f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BrellaShotgunProjectile>(),
                singleShotTime: 32,
                shotVelocity: 8f);

            Item.damage = 9;
            Item.width = 50;
            Item.height = 58;
            Item.knockBack = 2;
        }
    }
}
