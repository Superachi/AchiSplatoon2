using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.BlasterProjectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    internal class BaseBlaster : BaseWeapon
    {
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Blaster;
        public override float InkCost { get => 8f; }
        public override float InkRecoveryDelay { get => 20f; }

        // Audio
        public override SoundStyle ShootSample { get => SoundPaths.BlasterFire.ToSoundStyle(); }
        public virtual SoundStyle ExplosionBigSample { get => SoundPaths.BlasterExplosion.ToSoundStyle(); }
        public virtual SoundStyle ExplosionSmallSample { get => SoundPaths.BlasterExplosionLight.ToSoundStyle(); }

        // Explosion radius and delay
        public virtual int ExplosionRadiusAir { get => 100; }
        public virtual float ExplosionDelayInit { get => 5f; }

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BlasterProjectile>(),
                singleShotTime: 15,
                shotVelocity: 4f);

            Item.damage = 10;
            Item.autoReuse = true;
            Item.noMelee = true;
        }
    }
}
