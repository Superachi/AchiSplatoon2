using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class Dot52Gal : BaseSplattershot
    {
        public override float InkCost { get => 2.5f; }

        public virtual int DamageOverride => 52;
        protected override string UsageHintParamA => $"{DamageOverride}";

        public override SoundStyle ShootSample { get => SoundPaths.Dot52GalShoot.ToSoundStyle(); }
        public override float MuzzleOffsetPx { get; set; } = 48f;
        public override Vector2? HoldoutOffset() { return new Vector2(-2, 0); }

        public override float ShotGravity { get => 0.5f; }
        public override int ShotGravityDelay => 10;
        public override int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 12f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<Dot52GalProjectile>(),
                singleShotTime: 10,
                shotVelocity: 8f);

            Item.damage = 52;
            Item.width = 52;
            Item.height = 30;
            Item.knockBack = 6f;
            Item.value = Item.buyPrice(gold: 8);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.GelBalloon, 15);
            recipe.AddIngredient(ItemID.PinkGel, 5);
            recipe.Register();
        }
    }
}
