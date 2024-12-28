using AchiSplatoon2.Content.Projectiles.SpecialProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class KillerWail : BaseSpecial
    {
        public override bool IsDurationSpecial => true;
        public override float SpecialDrainPerTick => 0.235f;
        public override float RechargeCostPenalty { get => 80f; }

        private static readonly int ArmorPierce = 10;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ArmorPierce);

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<KillerWailShooter>(),
                singleShotTime: 30,
                shotVelocity: -10f);

            Item.damage = 50;
            Item.knockBack = 1;
            Item.ArmorPenetration = ArmorPierce;

            Item.width = 24;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTurn = true;
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true);
    }
}
