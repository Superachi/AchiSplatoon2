using AchiSplatoon2.Content.Projectiles.SplatlingProjectiles.Charges;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class MiniSplatling : BaseSplatling
    {
        public override Vector2? HoldoutOffset() { return new Vector2(-20, 12); }
        public override float MuzzleOffsetPx { get; set; } = 52f;
        public override float[] ChargeTimeThresholds { get => [22f, 34f]; }
        public override float BarrageVelocity { get; set; } = 8f;
        public override int BarrageShotTime { get; set; } = 4;
        public override int BarrageMaxAmmo { get; set; } = 24;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<HeavySplatlingCharge>(),
                singleShotTime: BarrageShotTime + 10,
                shotVelocity: BarrageVelocity);
            Item.damage = 10;
            Item.width = 72;
            Item.height = 28;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseBasic(registerNow: false);
            recipe.AddIngredient(ItemID.DemoniteBar, 5);
            recipe.Register();

            var altRecipe = AddRecipeWithSheldonLicenseBasic(registerNow: false);
            altRecipe.AddIngredient(ItemID.CrimtaneBar, 5);
            altRecipe.Register();
        }
    }
}
