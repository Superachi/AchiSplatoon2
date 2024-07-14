using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class DarkTetraDualie : SplatDualie
    {
        public override float ShotGravity { get => 0.15f; }
        public override int ShotGravityDelay { get => 30; }
        public override int ShotExtraUpdates { get => 3; }
        public override float AimDeviation { get => 6f; }
        public override string ShootSample { get => "SplatlingShoot"; }
        public override Vector2? HoldoutOffset() { return new Vector2(-4, 0); }
        public override float MuzzleOffsetPx => 42f;
        public override string RollSample { get => "Dualies/TetraDualieRoll"; }


        // Dualie specific
        public override float PostRollDamageMod { get => 1.2f; }
        public override float PostRollAttackSpeedMod { get => 0.9f; }
        public override float PostRollAimMod { get => 0.25f; }
        public override float PostRollVelocityMod { get => 1.2f; }
        public override int MaxRolls => 4;
        public override bool PreventShootOnRoll => false;
        public override float RollDistance { get => 14f; }
        public override float RollDuration { get => 20f; }
        public override bool SlowMoveAfterRoll => false;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<DualieShotProjectile>(),
                singleShotTime: 6,
                shotVelocity: 5f);

            Item.damage = 22;
            Item.width = 56;
            Item.height = 38;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(gold: 8);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes()
        {
            AddRecipeWithSheldonLicenseSilver(registerNow: false)
                .AddIngredient(ItemID.MythrilBar, 5)
                .AddIngredient(ItemID.SoulofNight, 5)
                .Register();
        }
    }
}
