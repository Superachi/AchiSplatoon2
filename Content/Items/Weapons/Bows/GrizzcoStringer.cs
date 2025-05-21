using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class GrizzcoStringer : TriStringer
    {
        public override float InkCost { get => 2f; }

        public override float[] ChargeTimeThresholds { get => [30f, 62f]; }
        public override SoundStyle ShootSample { get => SoundPaths.WellspringShoot.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.WellspringShootWeak.ToSoundStyle(); }
        public override float ShotgunArc { get => 110f; }
        public override int ProjectileCount { get => 9; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<TriStringerCharge>(),
                singleShotTime: 28,
                shotVelocity: 0);

            Item.damage = 80;
            Item.width = 34;
            Item.height = 74;
            Item.knockBack = 4;
            Item.SetValuePostPlantera();
            Item.crit = 15;
        }

        public override void AddRecipes() => AddRecipeGrizzco(ModContent.ItemType<TriStringer>());
    }
}
