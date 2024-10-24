using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class Dot52Gal : BaseSplattershot
    {
        public virtual int DamageOverride => 52;
        protected override string UsageHintParamA => $"{DamageOverride}";

        public override string ShootSample { get => "Dot52GalShoot"; }
        public override float MuzzleOffsetPx { get; set; } = 48f;
        public override Vector2? HoldoutOffset() { return new Vector2(-2, 0); }

        public override float ShotGravity { get => 0.5f; }
        public override int ShotGravityDelay => 10;
        public override int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 8f; }

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

        public override bool AllowPrefix(int pre)
        {
            switch (pre)
            {
                case PrefixID.Quick:
                case PrefixID.Agile:
                case PrefixID.Nimble:
                case PrefixID.Rapid:
                case PrefixID.Hasty:
                case PrefixID.Lethargic:
                case PrefixID.Awkward:
                    return true;
            }

            return false;
        }
    }
}
