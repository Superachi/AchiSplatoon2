using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class SlimeSplattershot : Splattershot
    {
        public override float InkCost { get => 3f; }

        public override SoundStyle ShootSample { get => SoundPaths.Silence.ToSoundStyle(); }
        public override float ShotGravity { get => 0.3f; }
        public override int ShotGravityDelay { get => 30; }
        public override int ShotExtraUpdates { get => 2; }
        public override float AimDeviation { get => 2f; }
        public override Vector2? HoldoutOffset() { return new Vector2(-8, 0); }
        public override Vector2 MuzzleOffset => new Vector2(44f, 0);

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SlimeSplattershotProjectile>(),
                singleShotTime: 24,
                shotVelocity: 7f);

            Item.damage = 14;
            Item.width = 42;
            Item.height = 26;
            Item.knockBack = 5;
            Item.SetValuePreEvilBosses();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Gel, 99)
                .AddIngredient(ItemID.WaterGun, 1)
                .AddIngredient(ModContent.ItemType<InkDroplet>(), 5)
                .Register();
        }
    }
}
