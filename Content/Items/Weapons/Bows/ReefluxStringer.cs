using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class ReefluxStringer : TriStringer
    {
        public override float[] ChargeTimeThresholds { get => [20f, 40f]; }
        public override float ShotgunArc { get => 5f; }
        public override int ProjectileCount { get => 3; }
        public override bool AllowStickyProjectiles { get => false; }
        public override Vector2? HoldoutOffset() { return new Vector2(-4, 2); }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToRangedWeapon(
                baseProjType: ModContent.ProjectileType<TriStringerCharge>(),
                ammoID: AmmoID.None,
                singleShotTime: 12,
                shotVelocity: 12f);

            Item.width = 36;
            Item.height = 62;
            Item.damage = 16;
            Item.knockBack = 1.5f;
            Item.value = Item.buyPrice(gold: 3);
            Item.crit = 10;
        }

        public override void AddRecipes()
        {
            AddRecipeWithSheldonLicenseBasic();
        }
    }
}
