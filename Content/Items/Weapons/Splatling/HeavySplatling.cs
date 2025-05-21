using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.SplatlingProjectiles.Charges;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class HeavySplatling : BaseSplatling
    {
        public override SoundStyle ShootSample { get => SoundPaths.HeavySplatlingShoot.ToSoundStyle(); }
        public override float InkCost { get => 2f; }

        public override Vector2? HoldoutOffset() { return new Vector2(-46, 6); }
        public override Vector2 MuzzleOffset => new Vector2(42f, 0);
        public override float[] ChargeTimeThresholds { get => [50f, 75f]; }
        public override float BarrageVelocity { get; set; } = 12f;
        public override int BarrageShotTime { get; set; } = 4;
        public override int BarrageMaxAmmo { get; set; } = 32;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<HeavySplatlingCharge>(),
                singleShotTime: BarrageShotTime + 10,
                shotVelocity: BarrageVelocity);
            Item.damage = 16;
            Item.width = 92;
            Item.height = 50;
            Item.knockBack = 3;
            Item.SetValueLatePreHardmode();
        }

        public override void AddRecipes() => AddRecipePostSkeletron();
    }
}
