using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using AchiSplatoon2.Content.Projectiles;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    internal class ClassicSquiffer : SplatCharger
    {
        public override string ShootSample { get => "SquifferChargerShoot"; }
        public override string ShootWeakSample { get => "SquifferChargerShootWeak"; }
        public override bool ScreenShake => false;
        public override float[] ChargeTimeThresholds { get => [42f]; }
        public override float RangeModifier => 0.25f;
        public override float MinPartialRange { get => 0.3f; }
        public override float MaxPartialRange { get => 0.6f; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SplatChargerProjectile>(),
                singleShotTime: 12,
                shotVelocity: 12f);

            Item.width = 90;
            Item.height = 26;
            Item.damage = 200;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.CobaltBar, 5);
            recipe.Register();
        }
    }
}
