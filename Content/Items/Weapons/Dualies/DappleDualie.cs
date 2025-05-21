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
    internal class DappleDualie : SplatDualie
    {
        public override SoundStyle ShootSample { get => SoundPaths.DualieDappleShoot.ToSoundStyle(); }
        public override SoundStyle ShootAltSample { get => ShootSample; }

        // Shoot settings
        public override float ShotGravity { get => 0.3f; }
        public override int ShotGravityDelay { get => 4; }
        public override int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 10f; }
        public override Vector2? HoldoutOffset() { return new Vector2(-20, 0); }
        public override Vector2 MuzzleOffset => new Vector2(52f, 0);

        // Dualie specific
        public override SoundStyle RollSample { get => SoundPaths.DualieTetraRoll.ToSoundStyle(); }
        public override float RollInkCost { get => 3f; }
        public override float PostRollAimMod { get => 0.5f; }
        public override float PostRollVelocityMod { get => 1.2f; }
        public override int MaxRolls { get => 2; }
        public override float RollDistance { get => 15f; }
        public override float RollDuration { get => 15f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<DualieShotProjectile>(),
                singleShotTime: 6,
                shotVelocity: 3f);

            Item.damage = 10;
            Item.crit = 5;
            Item.width = 50;
            Item.height = 36;
            Item.knockBack = 2;

            Item.SetValuePostEvilBosses();
        }

        public override void AddRecipes() => AddRecipePostBee();
    }
}
