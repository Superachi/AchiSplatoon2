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
    internal class CoralStringer : BaseStringer
    {
        public override float InkCost { get => 2f; }
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Stringer;
        public override Vector2? HoldoutOffset() { return new Vector2(-12, -2); }

        public override float[] ChargeTimeThresholds { get => [20f, 40f]; }
        public override SoundStyle ShootSample { get => SoundPaths.Silence.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.Silence.ToSoundStyle(); }
        public override float ShotgunArc { get => 30f; }
        public override int ProjectileCount { get => 4; }
        public override bool AllowStickyProjectiles { get => false; }

        public override int ProjectileType => ModContent.ProjectileType<CoralStringerProjectile>();

        public virtual bool CanShotBounce => false;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<CoralStringerCharge>(),
                singleShotTime: 10,
                shotVelocity: 1);

            Item.width = 36;
            Item.height = 62;
            Item.damage = 20;
            Item.knockBack = 6;

            Item.SetValuePreEvilBosses();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GoldBow, 1)
                .AddIngredient(ItemID.WhiteString, 2)
                .AddIngredient(ItemID.Coral, 20)
                .AddIngredient(ItemID.SharkFin, 1)
                .AddIngredient(ItemID.FallenStar, 1)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.PlatinumBow, 1)
                .AddIngredient(ItemID.WhiteString, 2)
                .AddIngredient(ItemID.Coral, 20)
                .AddIngredient(ItemID.SharkFin, 1)
                .AddIngredient(ItemID.FallenStar, 1)
                .Register();
        }
    }
}
