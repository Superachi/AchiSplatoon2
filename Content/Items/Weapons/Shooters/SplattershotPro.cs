using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class SplattershotPro : BaseSplattershot
    {
        public override float InkCost { get => 2f; }

        public override SoundStyle ShootSample { get => SoundPaths.JetSquelcherShoot.ToSoundStyle(); }
        public override float ShotGravity { get => 0.2f; }
        public override int ShotGravityDelay => 20;
        public override int ShotExtraUpdates { get => 8; }
        public override Vector2 MuzzleOffset => new Vector2(56f, 0);
        public override Vector2? HoldoutOffset() { return new Vector2(-20, -2); }
        public override float AimDeviation { get => 2f; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SplattershotProjectile>(),
                singleShotTime: 8,
                shotVelocity: 6f);

            Item.damage = 16;
            Item.width = 64;
            Item.height = 36;
            Item.knockBack = 3;
            Item.SetValueLowHardmodeOre();
        }

        public override void AddRecipes() => AddRecipeHellstone();
    }
}
