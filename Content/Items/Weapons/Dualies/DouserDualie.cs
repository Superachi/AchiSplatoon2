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
    internal class DouserDualie : SplatDualie
    {
        // Shoot settings
        public override float ShotGravity { get => 0.3f; }
        public override int ShotGravityDelay { get => 20; }
        public override int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 2f; }
        public override string ShootSample { get => "SplatlingShoot"; }
        public override string ShootAltSample { get => ShootSample; }
        public override Vector2? HoldoutOffset() { return new Vector2(0, 0); }
        public override float MuzzleOffsetPx { get; set; } = 52f;

        // Dualie specific
        public override string RollSample { get => "Dualies/SplatDualieRoll"; }
        public override float PostRollDamageMod { get => 1.2f; }
        public override float PostRollAttackSpeedMod { get => 0.5f; }
        public override float PostRollAimMod { get => 0.25f; }
        public override float PostRollVelocityMod { get => 0.5f; }
        public override int MaxRolls { get => 1; }
        public override float RollDistance { get => 15f; }
        public override float RollDuration { get => 36f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<DualieShotProjectile>(),
                singleShotTime: 8,
                shotVelocity: 10f);

            Item.damage = 28;
            Item.width = 52;
            Item.height = 34;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            AddRecipeWithSheldonLicenseSilver(registerNow: false)
                .AddIngredient(ItemID.TitaniumBar, 5)
                .Register();
        }
    }
}
