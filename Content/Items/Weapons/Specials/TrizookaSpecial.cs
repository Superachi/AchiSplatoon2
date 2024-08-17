using AchiSplatoon2.Content.Projectiles.SpecialProjectiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class TrizookaSpecial : BaseSpecial
    {
        public override string ShootSample { get => "Specials/TrizookaLaunch"; }
        public override Vector2? HoldoutOffset() { return new Vector2(-40, -8); }
        public override float MuzzleOffsetPx { get; set; } = 80f;
        public static readonly int ProjPerShot = 3;
        public static readonly int MaxBursts = 3;
        protected override string UsageHintParamA => ProjPerShot.ToString();
        protected override string UsageHintParamB => MaxBursts.ToString();
        public override float SpecialDrainPerTick => 0.1f;
        public override float SpecialDrainPerUse => 55f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<TrizookaShooter>(),
                singleShotTime: 50,
                shotVelocity: 20f
            );

            Item.damage = 75;
            Item.width = 90;
            Item.height = 44;
            Item.knockBack = 10;
        }

        public override void AddRecipes() => AddRecipePostEOC();
    }
}
