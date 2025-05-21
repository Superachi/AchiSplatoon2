using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class Squeezer : BaseSplattershot
    {
        public override float InkCost { get => 3f; }
        public override float ShotGravity { get => 0.4f; }
        public override int ShotGravityDelay => 20;
        public override int ShotExtraUpdates { get => 6; }
        public override float AimDeviation { get => 2f; }
        public override SoundStyle ShootSample { get => SoundPaths.SqueezerShoot.ToSoundStyle(); }
        public override SoundStyle ShootAltSample { get => SoundPaths.SqueezerShootAlt.ToSoundStyle(); }

        public override Vector2? HoldoutOffset() { return new Vector2(0, 0); }
        public override Vector2 MuzzleOffset => new Vector2(58, -6);

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SqueezerProjectile>(),
                singleShotTime: 9,
                shotVelocity: 9f);

            Item.damage = 56;
            Item.crit = 5;
            Item.knockBack = 6f;

            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes()
        {
        }
    }
}
