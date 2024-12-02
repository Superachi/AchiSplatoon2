using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.BrellaProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class BaseBrella : BaseWeapon
    {
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Brella;
        public override float InkCost { get => 5f; }
        public override float InkRecoveryDelay { get => 20f; }

        public virtual float ShotGravity { get => 0.4f; }
        public virtual int ShotGravityDelay { get => 18; }
        public virtual int ShotExtraUpdates { get => 3; }
        public override float AimDeviation { get => 6f; }
        public override SoundStyle ShootSample { get => SoundPaths.BrellaShot.ToSoundStyle(); }
        public override Vector2? HoldoutOffset() { return new Vector2(4, 0); }
        public override float MuzzleOffsetPx { get; set; } = 44f;

        // Brella specific
        public virtual int ProjectileType { get => ModContent.ProjectileType<BrellaPelletProjectile>(); }
        public virtual int MeleeProjectileType { get => ModContent.ProjectileType<BrellaMeleeProjectile>(); }
        public virtual int ProjectileCount { get => 10; }
        public virtual float ShotgunArc { get => 3f; }
        public virtual float ShotVelocityRandomRange => 0.2f;
        public virtual int ShieldLife => 200;
        public virtual int ShieldCooldown => 300;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs($"{ProjectileCount}");

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

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var accMP = player.GetModPlayer<AccessoryPlayer>();
            var brellaMP = player.GetModPlayer<BrellaPlayer>();

            var p = CreateProjectileWithWeaponProperties(
                player: player,
                source: source,
                velocity: velocity,
                triggerSpawnMethods: false);
            var proj = p as BrellaShotgunProjectile;

            if (!brellaMP.shieldAvailable && accMP.hasMarinatedNecklace)
            {
                player.itemTime = (int)(player.itemTime * MarinatedNecklace.RecoverAttackSpeedModifier);
            }
            proj.RunSpawnMethods();

            return false;
        }
    }
}
