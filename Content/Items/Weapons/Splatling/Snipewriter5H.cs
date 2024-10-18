using AchiSplatoon2.Content.Projectiles.SplatlingProjectiles.Charges;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class Snipewriter5H : BaseSplatling
    {
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Charger;
        public override string ShootSample { get => "SnipewriterShoot"; }

        public override Vector2? HoldoutOffset() { return new Vector2(-24, 0); }
        public override float MuzzleOffsetPx { get; set; } = 52f;
        public override float[] ChargeTimeThresholds { get => [85f]; }
        public override float BarrageVelocity { get; set; } = 3f;
        public override int BarrageShotTime { get; set; } = 15;
        public override int BarrageMaxAmmo { get; set; } = 5;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SnipewriterCharge>(),
                singleShotTime: BarrageShotTime + 10,
                shotVelocity: BarrageVelocity);
            Item.damage = 120;
            Item.width = 88;
            Item.height = 26;
            Item.crit = 5;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.Pearlwood, 120);
            recipe.AddIngredient(ItemID.LeadBar, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ItemID.Lens, 5);
            recipe.Register();
        }
    }
}
