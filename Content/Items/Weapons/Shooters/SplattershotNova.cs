using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters;

internal class SplattershotNova : BaseSplattershot
{
    public override float InkCost { get => 0.8f; }

    public override SoundStyle ShootSample { get => SoundPaths.MiniSplatlingShoot.ToSoundStyle(); }
    public override float ShotGravity { get => 0.2f; }
    public override int ShotGravityDelay => 15;
    public override int ShotExtraUpdates { get => 6; }
    public override Vector2 MuzzleOffset => new Vector2(56f, -4);
    public override Vector2? HoldoutOffset() { return new Vector2(-16, -2); }
    public override float AimDeviation { get => 3f; }
    public override void SetDefaults()
    {
        base.SetDefaults();
        RangedWeaponDefaults(
            projectileType: ModContent.ProjectileType<SplattershotProjectile>(),
            singleShotTime: 7,
            shotVelocity: 6f);

        Item.ArmorPenetration = 5;
        Item.damage = 6;
        Item.width = 64;
        Item.height = 36;
        Item.knockBack = 1.5f;
        Item.SetValuePostEvilBosses();
    }

    public override void AddRecipes() => AddRecipeMeteorite();
}