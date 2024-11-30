using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Projectiles.BrellaProjectiles.CorruptionBrellaProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class CorruptionBrella : BaseBrella
    {
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Brella;
        public override float InkCost { get => 6.5f; }

        public override float ShotGravity { get => 0f; }
        public override int ShotGravityDelay { get => 0; }
        public override int ShotExtraUpdates { get => 2; }
        public override float AimDeviation { get => 0; }
        public override string ShootSample { get => "Brellas/BrellaShot"; }
        public override Vector2? HoldoutOffset() { return new Vector2(4, 0); }
        public override float MuzzleOffsetPx { get; set; } = 44f;

        // Brella specific
        public override int ProjectileType { get => ModContent.ProjectileType<CorruptionBrellaPelletProjectile>(); }
        public override int MeleeProjectileType { get => ModContent.ProjectileType<CorruptionBrellaMeleeProjectile>(); }
        public override int ProjectileCount { get => 8; }
        public override float ShotgunArc { get => 360f; }
        public override float ShotVelocityRandomRange => 0.3f;
        public override int ShieldLife => 200;
        public override int ShieldCooldown => 300;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<CorruptionBrellaShotgunProjectile>(),
                singleShotTime: 30,
                shotVelocity: 4f);

            Item.damage = 10;
            Item.width = 54;
            Item.height = 64;
            Item.knockBack = 9;
            Item.ArmorPenetration = 10;

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Umbrella)
                .AddIngredient(ItemID.ShadowScale, 15)
                .AddIngredient(ItemID.Obsidian, 30)
                .AddIngredient(ItemID.Stinger, 5)
                .AddIngredient(ItemID.Lens, 1)
                .Register();
        }
    }
}
