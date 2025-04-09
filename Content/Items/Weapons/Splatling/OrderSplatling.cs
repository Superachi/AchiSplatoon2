using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Projectiles.SplatlingProjectiles.Charges;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    [OrderWeapon]
    internal class OrderSplatling : BaseSplatling
    {
        public override float InkCost { get => 2f; }
        public override Vector2? HoldoutOffset() { return new Vector2(-36, 8); }
        public override Vector2 MuzzleOffset => new Vector2(42f, 0);
        public override float[] ChargeTimeThresholds { get => [75f, 100f]; }
        public override float BarrageVelocity { get; set; } = 8f;
        public override int BarrageShotTime { get; set; } = 6;
        public override int BarrageMaxAmmo { get; set; } = 20;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<HeavySplatlingCharge>(),
                singleShotTime: BarrageShotTime + 10,
                shotVelocity: BarrageVelocity);

            Item.damage = 10;
            Item.crit = 0;
            Item.width = 78;
            Item.height = 42;
            Item.knockBack = 1f;

            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes() => AddRecipeOrder(ItemID.Diamond);
    }
}
