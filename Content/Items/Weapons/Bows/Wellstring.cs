using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class Wellstring : TriStringer
    {
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Stringer;

        public override Vector2? HoldoutOffset() { return new Vector2(-8, 2); }
        public override float[] ChargeTimeThresholds { get => [40f, 80f]; }
        public override SoundStyle ShootSample { get => SoundPaths.WellspringShoot.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.WellspringShootWeak.ToSoundStyle(); }
        public override float ShotgunArc { get => 10f; }
        public override int ProjectileCount { get => 5; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<WellstringCharge>(),
                singleShotTime: 18,
                shotVelocity: 0);

            Item.damage = 110;
            Item.width = 42;
            Item.height = 74;
            Item.knockBack = 6;
            Item.SetValuePostMech();
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofMight);
    }
}
