using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class DualieSquelcher : BaseDualie
    {
        public override float ShotGravity { get => 0.1f; }
        public override int ShotGravityDelay { get => 20; }
        public override int ShotExtraUpdates { get => 8; }
        public override float AimDeviation { get => 3f; }
        public override string ShootSample { get => "SplatlingShoot"; }
        public override Vector2? HoldoutOffset() { return new Vector2(-2, 4); }
        public override float MuzzleOffsetPx { get; set; } = 54f;

        // Dualie specific
        public override string RollSample { get => "Dualies/TetraDualieRoll"; }
        public override float PostRollDamageMod { get => 1f; }
        public override float PostRollAttackSpeedMod { get => 1f; }
        public override float PostRollAimMod { get => 0.5f; }
        public override float PostRollVelocityMod { get => 1f; }
        public override int MaxRolls { get => 2; }
        public override float RollDistance { get => 12f; }
        public override float RollDuration { get => 16f; }
        public override bool SlowMoveAfterRoll { get => false; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<DualieShotProjectile>(),
                singleShotTime: 6,
                shotVelocity: 6f);

            Item.damage = 36;
            Item.width = 50;
            Item.height = 36;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipeChlorophyteUpgrade(true, ModContent.ItemType<JetSquelcher>());
    }
}
