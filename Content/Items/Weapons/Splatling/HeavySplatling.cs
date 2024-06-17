using AchiSplatoon2.Content.Projectiles.SplatlingProjectiles.Charges;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class HeavySplatling : BaseSplatling
    {
        public override Vector2? HoldoutOffset() { return new Vector2(-46, 6); }
        public override float MuzzleOffsetPx { get; set; } = 50f;
        public override float[] ChargeTimeThresholds { get => [50f, 75f]; }
        public override float BarrageVelocity { get; set; } = 12f;
        public override int BarrageShotTime { get; set; } = 4;
        public override int BarrageMaxAmmo { get; set; } = 32;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToRangedWeapon(
                baseProjType: ModContent.ProjectileType<HeavySplatlingCharge>(),
                ammoID: AmmoID.None,
                singleShotTime: BarrageShotTime + 10,
                shotVelocity: BarrageVelocity);
            Item.damage = 18;
            Item.width = 92;
            Item.height = 50;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Minishark, 1);
            recipe.AddIngredient(ItemID.HellstoneBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
