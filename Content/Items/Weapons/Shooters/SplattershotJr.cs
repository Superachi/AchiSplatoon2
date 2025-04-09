using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class SplattershotJr : BaseSplattershot
    {
        public override float InkCost { get => 0.7f; }
        public virtual float InkTankCapacityBonus { get => 20f; }
        protected override string UsageHintParamA => InkTankCapacityBonus.ToString("0");

        public override float ShotGravity { get => 0.4f; }
        public override int ShotGravityDelay { get => 4; }
        public override int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 8f; }
        public override SoundStyle ShootSample { get => SoundPaths.SplattershotShoot.ToSoundStyle(); }
        public override Vector2? HoldoutOffset() { return new Vector2(-8, -2); }
        public override Vector2 MuzzleOffset => new Vector2(44f, 0);

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SplattershotProjectile>(),
                singleShotTime: 7,
                shotVelocity: 4f);

            Item.damage = 6;
            Item.width = 48;
            Item.height = 30;
            Item.knockBack = 0.5f;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;

            Item.ArmorPenetration = 3;
        }

        public override void AddRecipes() => AddRecipeWithSheldonLicenseBasic();
    }
}
