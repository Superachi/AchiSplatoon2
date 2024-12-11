using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.ShooterProjectiles.NozzlenoseProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class L3Nozzlenose : BaseSplattershot
    {
        public override float InkCost { get => 3f; }

        public virtual float ShotVelocity { get => 8f; }
        public virtual int BurstShotTime { get => 4; }
        public virtual float DamageIncreasePerHit { get => 0.5f; }

        public override float ShotGravity { get => 0.4f; }
        public override int ShotGravityDelay { get => 20; }
        public override int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 2f; }
        public override SoundStyle ShootSample { get => SoundPaths.SplattershotShoot.ToSoundStyle(); }
        public override Vector2? HoldoutOffset() { return new Vector2(-2, 0); }
        public override float MuzzleOffsetPx { get; set; } = 48f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<NozzlenoseShooter>(),
                singleShotTime: 14,
                shotVelocity: 1f);

            Item.damage = 24;
            Item.width = 50;
            Item.height = 32;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipeTitanium();
    }
}
