using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using AchiSplatoon2.Content.Projectiles.SlosherProjectiles;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class Bloblobber : BaseSlosher
    {
        public override string ShootSample { get => "Sloshers/BloblobberShoot"; }
        public override string ShootWeakSample { get => "Sloshers/BloblobberShootAlt"; }
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

            Item.damage = 40;
            Item.width = 38;
            Item.height = 28;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.LightPurple;
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
