using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class GrizzcoDualie : SplatDualie
    {
        public override float ShotGravity { get => 0.3f; }
        public override int ShotGravityDelay { get => 12; }
        public override int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 8f; }
        public override Vector2? HoldoutOffset() { return new Vector2(-8, 0); }
        public override float MuzzleOffsetPx { get; set; } = 50f;

        // Dualie specific
        public override float RollInkCost { get => 3f; }
        public override int MaxRolls { get => 8; }
        public override float RollDistance { get => 15f; }
        public override float RollDuration { get => 15f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<DualieShotProjectile>(),
                singleShotTime: 5,
                shotVelocity: 3.5f);

            Item.damage = 32;
            Item.width = 56;
            Item.height = 38;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeGrizzco(ModContent.ItemType<SplatDualie>());
    }
}
