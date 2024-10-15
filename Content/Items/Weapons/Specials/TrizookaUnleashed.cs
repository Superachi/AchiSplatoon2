using AchiSplatoon2.Content.Items.CraftingMaterials;
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
                singleShotTime: 25,
                shotVelocity: 14f
            );

            Item.rare = ItemRarityID.Cyan;
            Item.damage = 100;
            Item.knockBack = 10;
            Item.value = Item.buyPrice(gold: 50);
        }

        public override bool CanReforge() => true;

        public override bool AllowPrefix(int pre) => true;

        public override void AddRecipes()
        {
            var recipe = CreateRecipe()
                .AddIngredient(ModContent.ItemType<SheldonLicenseGold>(), 10)
                .AddIngredient(ItemID.ChlorophyteBar, 5)
                .AddIngredient(ItemID.ShroomiteBar, 5)
                .AddIngredient(ItemID.SpectreBar, 5)
                .AddIngredient(ModContent.ItemType<TrizookaSpecial>(), 1)
                .Register();
        }
    }
}
