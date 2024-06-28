using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.Content.Projectiles.SplatlingProjectiles.Charges;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    internal class BaseBlaster : BaseWeapon
    {
        // Audio
        public override string ShootSample { get => "BlasterShoot"; }
        public virtual string ExplosionBigSample { get => "BlasterExplosion"; }
        public virtual string ExplosionSmallSample { get => "BlasterExplosionLight"; }

        // Explosion radius and delay
        public virtual int ExplosionRadiusAir { get => 100; }
        public virtual int ExplosionRadiusTile { get => 50; }
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
