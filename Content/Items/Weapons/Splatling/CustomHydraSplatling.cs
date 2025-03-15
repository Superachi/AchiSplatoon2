using AchiSplatoon2.Content.Projectiles.SplatlingProjectiles.Charges;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class CustomHydraSplatling : HydraSplatling
    {
        public override float[] ChargeTimeThresholds { get => [120f, 150f]; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<HeavySplatlingCharge>(),
                singleShotTime: BarrageShotTime + 10,
                shotVelocity: BarrageVelocity);
            Item.damage = 58;
            Item.width = 88;
            Item.height = 50;
            Item.knockBack = 6;

            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes()
        {
        }
    }
}
