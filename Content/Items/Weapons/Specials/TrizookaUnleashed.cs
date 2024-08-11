using AchiSplatoon2.Content.Projectiles.SpecialProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class TrizookaUnleashed : TrizookaSpecial
    {
        public override string ShootSample { get => "Specials/TrizookaLaunchAlly"; }
        public override float MuzzleOffsetPx { get; set; } = 80f;
        protected override string UsageHintParamA => "";
        protected override string UsageHintParamB => "";
        public override bool IsSpecialWeapon => false;
        public override bool AllowSubWeaponUsage => true;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<TrizookaShooter>(),
                singleShotTime: 50,
                shotVelocity: 20f
            );

            Item.rare = ItemRarityID.Cyan;
            Item.damage = 150;
            Item.knockBack = 10;
        }

        public override bool CanReforge() => true;

        public override bool AllowPrefix(int pre) => true;

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseGold(registerNow: false);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 5);
            recipe.AddIngredient(ItemID.ShroomiteBar, 5);
            recipe.AddIngredient(ItemID.SpectreBar, 5);
            recipe.AddIngredient(ModContent.ItemType<TrizookaSpecial>(), 1);
            recipe.Register();
        }
    }
}
