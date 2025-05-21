using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class Dot96Gal : Dot52Gal
    {
        public override float InkCost { get => 3.5f; }

        public override int DamageOverride => 68;
        public override SoundStyle ShootSample { get => SoundPaths.Dot96GalShoot.ToSoundStyle(); }
        public override Vector2 MuzzleOffset => new Vector2(60f, -4);
        public override Vector2? HoldoutOffset() { return new Vector2(-2, 0); }

        public override float ShotGravity { get => 0.45f; }
        public override int ShotGravityDelay => 12;
        public override float AimDeviation { get => 6f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<Dot52GalProjectile>(),
                singleShotTime: 12,
                shotVelocity: 9f);

            Item.damage = DamageOverride;
            Item.width = 62;
            Item.height = 32;
            Item.knockBack = 8f;
            Item.SetValuePostMech();
        }

        public override void AddRecipes() => AddRecipeChlorophyteUpgrade(true, ModContent.ItemType<Dot52Gal>());
    }
}
