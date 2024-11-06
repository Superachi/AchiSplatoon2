using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class SlimeSplattershot : Splattershot
    {
        public override string ShootSample => "Dot52GalShoot";
        public override float ShotGravity { get => 0.3f; }
        public override int ShotGravityDelay { get => 30; }
        public override int ShotExtraUpdates { get => 2; }
        public override float AimDeviation { get => 2f; }
        public override Vector2? HoldoutOffset() { return new Vector2(-8, 0); }
        public override float MuzzleOffsetPx { get; set; } = 44f;

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SlimeSplattershotProjectile>(),
                singleShotTime: 24,
                shotVelocity: 7f);

            Item.damage = 16;
            Item.width = 42;
            Item.height = 26;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes() { }
    }
}
