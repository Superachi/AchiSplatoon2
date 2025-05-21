using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class PainterDualie : BaseDualie
    {
        public override float InkCost { get => 2f; }
        public override float ShotGravity { get => 0.2f; }
        public override int ShotGravityDelay { get => 8; }
        public override int ShotExtraUpdates { get => 5; }
        public override float AimDeviation { get => 8f; }
        public override SoundStyle ShootSample { get => SoundPaths.SplattershotShoot.ToSoundStyle(); }
        public override Vector2? HoldoutOffset() { return new Vector2(-8, 0); }
        public override Vector2 MuzzleOffset => new Vector2(40f, 0);

        // Dualie specific
        public override SoundStyle RollSample { get => SoundPaths.DualieTetraRoll.ToSoundStyle(); }
        public override float RollInkCost { get => 3f; }
        public override float PostRollDamageMod { get => 2f; }
        public override float PostRollAttackSpeedMod { get => 1.5f; }
        public override float PostRollAimMod { get => 0.5f; }
        public override float PostRollVelocityMod { get => 1.5f; }
        public override int MaxRolls { get => 1; }
        public override float RollDistance { get => 10f; }
        public override float RollDuration { get => 14f; }
        public override bool SlowMoveAfterRoll { get => false; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<PainterDualieShotProjectile>(),
                singleShotTime: 10,
                shotVelocity: 4f);

            Item.damage = 10;
            Item.ArmorPenetration = 3;

            Item.width = 46;
            Item.height = 28;
            Item.knockBack = 1f;
            Item.SetValuePostEvilBosses();
        }
    }
}
