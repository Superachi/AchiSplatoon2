using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class SplatDualie : BaseDualie
    {
        public override float AimDeviation { get => 6f; }
        public override string ShootSample { get => "SplatlingShoot"; }
        public override Vector2? HoldoutOffset() { return new Vector2(-8, 0); }
        public override float MuzzleOffsetPx { get; set; } = 50f;

        // Dualie specific
        public override string RollSample { get => "Dualies/SplatDualieRoll"; }
        public override float PostRollDamageMod { get => 1.2f; }
        public override float PostRollAttackSpeedMod { get => 0.8f; }
        public override float PostRollAimMod { get => 0.25f; }
        public override float PostRollVelocityMod { get => 1.3f; }
        public override int MaxRolls { get => 2; }
        public override float RollDistance { get => 16f; }
        public override float RollDuration { get => 24f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<DualieShotProjectile>(),
                singleShotTime: 5,
                shotVelocity: 5f);

            Item.damage = 26;
            Item.width = 50;
            Item.height = 36;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes()
        {
            AddRecipeWithSheldonLicenseSilver(registerNow: false)
                .AddIngredient(ItemID.CrystalShard, 15)
                .Register();
        }
    }
}
