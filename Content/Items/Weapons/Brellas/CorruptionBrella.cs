using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Projectiles.BrellaProjectiles.CorruptionBrellaProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class CorruptionBrella : BaseBrella
    {
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Brella;
        public override float InkCost { get => 12.5f; }

        public override float ShotGravity { get => 0f; }
        public override int ShotGravityDelay { get => 0; }
        public override int ShotExtraUpdates { get => 2; }
        public override float AimDeviation { get => 0; }
        public override SoundStyle ShootSample { get => SoundPaths.Silence.ToSoundStyle(); }
        public override Vector2? HoldoutOffset() { return new Vector2(4, 0); }
        public override Vector2 MuzzleOffset => new Vector2(44f, 0);

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

            Item.damage = 5;
            Item.width = 54;
            Item.height = 64;
            Item.knockBack = 9;
            Item.ArmorPenetration = 10;

            Item.SetValuePostEvilBosses();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Umbrella)
                .AddIngredient(ItemID.ShadowScale, 15)
                .AddIngredient(ItemID.Stinger, 5)
                .AddIngredient(ModContent.ItemType<InkDroplet>(), 5)
                .Register();
        }
    }
}
