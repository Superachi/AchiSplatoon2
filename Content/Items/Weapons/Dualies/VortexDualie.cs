using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class VortexDualie : BaseDualie
    {
        // Shoot settings
        public override float ShotGravity { get => 0f; }
        public override int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 3f; }
        public override SoundStyle ShootSample { get => SoundPaths.AchiGunBlast.ToSoundStyle(); }
        public override SoundStyle ShootAltSample { get => ShootSample; }
        public override Vector2? HoldoutOffset() { return new Vector2(-8, 0); }
        public override float MuzzleOffsetPx { get; set; } = 36f;

        // Dualie specific
        public override SoundStyle RollSample { get => SoundPaths.AchiGunBoost.ToSoundStyle(); }
        public override float RollInkCost { get => 10f; }
        public override int RollProjectileType => ModContent.ProjectileType<VortexDualieRollProjectile>();
        public override float PostRollDamageMod { get => 1.5f; }
        public override float PostRollAttackSpeedMod { get => 0.8f; }
        public override float PostRollAimMod { get => 0.3f; }
        public override float PostRollVelocityMod { get => 1.5f; }
        public override int MaxRolls { get => 3; }
        public override float RollDistance { get => 8f; }
        public override float RollDuration { get => 12f; }
        public override bool SlowMoveAfterRoll { get => false; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<VortexDualieShotProjectile>(),
                singleShotTime: 6,
                shotVelocity: 6f);

            Item.damage = 50;
            Item.width = 50;
            Item.height = 30;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(gold: 50);
            Item.rare = ItemRarityID.Cyan;
        }
    }
}
