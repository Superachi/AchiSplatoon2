using AchiSplatoon2.Content.Projectiles.SplatlingProjectiles.Charges;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class HydraSplatling : BaseSplatling
    {
        public override string ShootSample { get => "JetSquelcherShoot"; }
        public override Vector2? HoldoutOffset() { return new Vector2(-44, 2); }
        public override float MuzzleOffsetPx { get; set; } = 52f;
        public override float[] ChargeTimeThresholds { get => [120f, 150f]; }
        public override float BarrageVelocity { get; set; } = 16f;
        public override int BarrageShotTime { get; set; } = 4;
        public override int BarrageMaxAmmo { get; set; } = 64;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<HeavySplatlingCharge>(),
                singleShotTime: BarrageShotTime + 10,
                shotVelocity: BarrageVelocity);
            Item.damage = 50;
            Item.width = 88;
            Item.height = 50;
            Item.knockBack = 6;

            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofSight);
    }
}
