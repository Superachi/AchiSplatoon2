using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class DouserDualie : SplatDualie
    {
        // Shoot settings
        public override float ShotGravity { get => 0.3f; }
        public override int ShotGravityDelay { get => 15; }
        public override int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 3f; }
        public override SoundStyle ShootAltSample { get => ShootSample; }
        public override Vector2? HoldoutOffset() { return new Vector2(0, 0); }
        public override Vector2 MuzzleOffset => new Vector2(52, -10);

        // Dualie specific
        public override float RollInkCost { get => 8f; }
        public override float PostRollDamageMod { get => 1.2f; }
        public override float PostRollAttackSpeedMod { get => 0.55f; }
        public override float PostRollAimMod { get => 0.25f; }
        public override float PostRollVelocityMod { get => 0.5f; }
        public override int MaxRolls { get => 1; }
        public override float RollDistance { get => 15f; }
        public override float RollDuration { get => 30f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<DualieShotProjectile>(),
                singleShotTime: 9,
                shotVelocity: 10f);

            Item.damage = 26;
            Item.width = 52;
            Item.height = 34;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipeTitanium();
    }
}
