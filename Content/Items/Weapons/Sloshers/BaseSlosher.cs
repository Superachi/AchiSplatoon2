using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.SlosherProjectiles;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    [ItemCategory("Slosher", "Sloshers")]
    internal class BaseSlosher : BaseWeapon
    {
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Slosher;
        public override float InkCost { get => 7.5f; }
        public override float InkRecoveryDelay { get => 20f; }

        public override SoundStyle ShootSample { get => SoundPaths.SlosherShoot.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.SlosherShootAlt.ToSoundStyle(); }

        public override float AimDeviation => 2f;
        public virtual float ShotGravity { get => 0.12f; }
        public virtual int ShotCount => 8;
        public virtual int Repetitions => 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SlosherMainProjectile>(),
                singleShotTime: 30,
                shotVelocity: 8f
            );
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            base.ModifyWeaponDamage(player, ref damage);
        }
    }
}
