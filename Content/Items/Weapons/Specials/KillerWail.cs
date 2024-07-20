using AchiSplatoon2.Content.Projectiles.SpecialProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class KillerWail : BaseSpecial
    {
        public override bool IsSpecialWeapon => true;
        public override bool IsDurationSpecial => true;
        public override float SpecialDrainPerTick => 0.2f;
        private static readonly int ArmorPierce = 10;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ArmorPierce);

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<KillerWailShooter>(),
                singleShotTime: 30,
                shotVelocity: -10f);

            Item.damage = 40;
            Item.knockBack = 1;
            Item.ArmorPenetration = ArmorPierce;

            Item.width = 24;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTurn = true;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseBasic(registerNow: false);
            recipe.AddIngredient(ItemID.MeteoriteBar, 5);
            recipe.Register();
        }
    }
}
