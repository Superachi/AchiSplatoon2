using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class BaseDualie : BaseWeapon
    {
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Dualies;

        protected override string UsageHintParamA => MaxRolls.ToString();

        // Shoot settings
        public virtual float ShotGravity { get => 0.3f; }
        public virtual int ShotGravityDelay { get => 20; }
        public virtual int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 8f; }
        public override string ShootSample { get => "SplatlingShoot"; }
        public override string ShootAltSample { get => ShootSample; }
        public override Vector2? HoldoutOffset() { return new Vector2(4, 0); }
        public override float MuzzleOffsetPx { get; set; } = 44f;

        // Dualie specific
        public virtual string RollSample { get => "Dualies/SplatDualieRoll"; }
        public virtual int RollProjectileType { get => ModContent.ProjectileType<DualieRollProjectile>(); }
        public virtual float PostRollDamageMod { get => 2f; }
        public virtual float PostRollAttackSpeedMod { get => 0.8f; }
        public virtual float PostRollAimMod { get => 0.25f; }
        public virtual float PostRollVelocityMod { get => 1.5f; }
        public virtual int MaxRolls { get => 4; }
        public virtual float RollDistance { get => 15f; }
        public virtual float RollDuration { get => 30f; }
        public virtual bool PreventShootOnRoll { get => true; }
        public virtual bool SlowMoveAfterRoll { get => true; }

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<DualieShotProjectile>(),
                singleShotTime: 6,
                shotVelocity: 6f);

            Item.damage = 5;
            Item.width = 50;
            Item.height = 36;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.White;
        }

        public override bool CanUseItem(Player player)
        {
            var dualieMP = player.GetModPlayer<DualiePlayer>();
            if (PreventShootOnRoll)
            {
                // Prevent shooting during a roll
                // The second if-statement is to prevent single shots during a chained roll (by holding down the jump key)
                if (dualieMP.isRolling || dualieMP.postRollCooldown > DualiePlayer.postRollCooldownDefault - 6)
                {
                    return false;
                }
            }

            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var dualieMP = player.GetModPlayer<DualiePlayer>();

            var p = CreateProjectileWithWeaponProperties(
                player: player,
                source: source,
                velocity: velocity,
                triggerSpawnMethods: false);
            var proj = p as DualieShotProjectile;

            if (dualieMP.isTurret)
            {
                player.itemTime = (int)(player.itemTime * PostRollAttackSpeedMod);
                p.Projectile.damage = (int)(p.Projectile.damage * PostRollDamageMod);
                p.Projectile.velocity *= PostRollVelocityMod;
                proj.aimDevOverride = AimDeviation * PostRollAimMod;
            }
            proj.RunSpawnMethods();

            return false;
        }
    }
}
