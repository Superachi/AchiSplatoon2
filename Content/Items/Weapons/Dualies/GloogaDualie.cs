using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class GloogaDualie : SplatDualie
    {
        public override float InkCost { get => 2f; }

        public override float AimDeviation { get => 6f; }
        public override SoundStyle ShootAltSample { get => SoundPaths.Dot96GalShoot.ToSoundStyle(); }
        public override Vector2? HoldoutOffset() { return new Vector2(-2, 0); }
        public override Vector2 MuzzleOffset => new Vector2(50f, 0);

        // Dualie specific
        public override SoundStyle RollSample { get => SoundPaths.DualieGloogaRoll.ToSoundStyle(); }
        public override float PostRollAttackSpeedMod { get => 1f; }
        public override float PostRollDamageMod { get => 1.6f; }
        public override float PostRollAimMod { get => 0.25f; }
        public override float PostRollVelocityMod { get => 1.5f; }
        public override int MaxRolls { get => 2; }
        public override float RollDistance { get => 14f; }
        public override float RollDuration { get => 28f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<DualieShotProjectile>(),
                singleShotTime: 10,
                shotVelocity: 6f);

            Item.damage = 34;
            Item.width = 40;
            Item.height = 30;
            Item.knockBack = 4;
            Item.SetValueHighHardmodeOre();
        }

        public override void AddRecipes() => AddRecipeAdamantite();
    }
}
