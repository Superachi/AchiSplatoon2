using AchiSplatoon2.Content.Projectiles.SplatlingProjectiles.Charges;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class HeavySplatling : BaseSplatling
    {
        public override float InkCost { get => 2f; }

        public override Vector2? HoldoutOffset() { return new Vector2(-46, 6); }
        public override float MuzzleOffsetPx { get; set; } = 42f;
        public override float[] ChargeTimeThresholds { get => [50f, 75f]; }
        public override float BarrageVelocity { get; set; } = 12f;
        public override int BarrageShotTime { get; set; } = 4;
        public override int BarrageMaxAmmo { get; set; } = 32;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<HeavySplatlingCharge>(),
                singleShotTime: BarrageShotTime + 10,
                shotVelocity: BarrageVelocity);
            Item.damage = 16;
            Item.width = 92;
            Item.height = 50;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(gold: 8);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes() => AddRecipePostSkeletron();
    }
}
