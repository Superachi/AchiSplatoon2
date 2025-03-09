using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
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
        public override float MuzzleOffsetPx { get; set; } = 56f;
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
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes() => AddRecipeHellstone();
    }
}
