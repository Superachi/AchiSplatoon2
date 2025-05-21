using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.SlosherProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class Bloblobber : BaseSlosher
    {
        public override SoundStyle ShootSample { get => SoundPaths.SlosherBloblobberShoot.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.SlosherBloblobberShootAlt.ToSoundStyle(); }

        public override float ShotGravity { get => 0.4f; }
        public virtual int BurstShotCount { get => 4; }
        public virtual int BurstShotDelay { get => 5; }

        public override void SetDefaults()
        {
            base.SetDefaults();

            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BloblobberMainProjectile>(),
                singleShotTime: 32,
                shotVelocity: 1f
            );
            Item.useStyle = ItemUseStyleID.DrinkLiquid;

            Item.damage = 44;
            Item.width = 38;
            Item.height = 28;
            Item.knockBack = 3;
            Item.SetValueHighHardmodeOre();
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.GelBalloon, 15);
            recipe.AddIngredient(ItemID.PinkGel, 5);
            recipe.Register();
        }
    }
}
