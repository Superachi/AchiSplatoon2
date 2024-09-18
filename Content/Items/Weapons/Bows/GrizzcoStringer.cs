using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class GrizzcoStringer : TriStringer
    {
        public override float[] ChargeTimeThresholds { get => [40f, 80f]; }
        public override float ShotgunArc { get => 90f; }
        public override int ProjectileCount { get => 9; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<TriStringerCharge>(),
                singleShotTime: 28,
                shotVelocity: 0);

            Item.damage = 80;
            Item.width = 34;
            Item.height = 74;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
            Item.crit = 15;
        }

        public override void AddRecipes() => AddRecipeGrizzco(ModContent.ItemType<TriStringer>());
    }
}
