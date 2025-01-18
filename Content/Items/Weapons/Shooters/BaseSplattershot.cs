using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    [ItemCategory("Shooter", "Shooters")]
    internal class BaseSplattershot : BaseWeapon
    {
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Shooter;
        public override float InkCost { get => 1f; }
        public override float InkRecoveryDelay { get => 12f; }

        public virtual float ShotGravity { get => 0.1f; }
        public virtual int ShotGravityDelay { get => 0; }
        public virtual int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 6f; }
        public override SoundStyle ShootSample { get => SoundPaths.SplattershotShoot.ToSoundStyle(); }
        public override Vector2? HoldoutOffset() { return new Vector2(4, 0); }
        public override float MuzzleOffsetPx { get; set; } = 44f;

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SplattershotProjectile>(),
                singleShotTime: 8,
                shotVelocity: 6f);
        }
    }
}
