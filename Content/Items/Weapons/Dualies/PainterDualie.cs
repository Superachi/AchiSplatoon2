using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class PainterDualie : BaseDualie
    {
        public override float ShotGravity { get => 0.1f; }
        public override int ShotGravityDelay { get => 12; }
        public override int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 8f; }
        public override string ShootSample { get => "SplattershotShoot"; }
        public override Vector2? HoldoutOffset() { return new Vector2(-8, 0); }
        public override float MuzzleOffsetPx { get; set; } = 40f;

        // Dualie specific
        public override string RollSample { get => "Dualies/TetraDualieRoll"; }
        public override float RollInkCost { get => 3f; }
        public override float PostRollDamageMod { get => 0.6f; }
        public override float PostRollAttackSpeedMod { get => 0.4f; }
        public override float PostRollAimMod { get => 0.5f; }
        public override float PostRollVelocityMod { get => 1f; }
        public override int MaxRolls { get => 1; }
        public override float RollDistance { get => 10f; }
        public override float RollDuration { get => 14f; }
        public override bool SlowMoveAfterRoll { get => false; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<PainterDualieShotProjectile>(),
                singleShotTime: 15,
                shotVelocity: 4f);

            Item.damage = 9;
            Item.ArmorPenetration = 3;

            Item.width = 46;
            Item.height = 28;
            Item.knockBack = 1f;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Green;
        }
    }
}
