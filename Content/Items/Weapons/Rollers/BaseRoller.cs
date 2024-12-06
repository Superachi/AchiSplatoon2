using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.RollerProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class BaseRoller : BaseWeapon
    {
        public override float InkCost { get => 12f; }
        public override float InkRecoveryDelay { get => 30f; }

        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Roller;
        public virtual SoundStyle WindUpSample { get => SoundPaths.RollerSwingMedium.ToSoundStyle(); }
        public virtual SoundStyle SwingSample { get => SoundPaths.RollerFling1.ToSoundStyle(); }

        public virtual float ShotGravity { get => 0.1f; }
        public virtual int ShotGravityDelay { get => 0; }
        public virtual int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 6f; }
        public override Vector2? HoldoutOffset() { return new Vector2(4, 0); }
        public override float MuzzleOffsetPx { get; set; } = 44f;

        // Roller specific
        public virtual float GroundWindUpDelayModifier => 1f;
        public virtual float GroundAttackVelocityModifier => 1f;
        public virtual float JumpWindUpDelayModifier => 1f;
        public virtual float JumpAttackDamageModifier => 1f;
        public virtual float JumpAttackVelocityModifier => 1f;
        public virtual float RollingAccelModifier => 1f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DamageType = DamageClass.Melee;
            Item.width = 56;
            Item.height = 50;
            Item.damage = 10;
            Item.useTime = 30;
            Item.useAnimation = Item.useTime;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<RollerSwingProjectile>();

            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[Item.shoot] > 0) return false;
            return base.CanUseItem(player);
        }
    }
}
