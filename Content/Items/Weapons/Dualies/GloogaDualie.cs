using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class GloogaDualie : SplatDualie
    {
        public override float AimDeviation { get => 6f; }
        public override SoundStyle ShootAltSample { get => SoundPaths.Dot52GalShoot.ToSoundStyle(); }
        public override Vector2? HoldoutOffset() { return new Vector2(8, 0); }
        public override float MuzzleOffsetPx { get; set; } = 50f;

        // Dualie specific
        public override SoundStyle RollSample { get => SoundPaths.DualieGloogaRoll.ToSoundStyle(); }
        public override float PostRollAttackSpeedMod { get => 1f; }
        public override float PostRollDamageMod { get => 1.5f; }
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
                singleShotTime: 9,
                shotVelocity: 6f);

            Item.damage = 36;
            Item.width = 40;
            Item.height = 30;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipeAdamantite();
    }
}
