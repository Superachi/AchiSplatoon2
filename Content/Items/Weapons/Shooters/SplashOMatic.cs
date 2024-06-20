using AchiSplatoon2.Content.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class SplashOMatic : SplooshOMatic
    {
        public override SubWeaponType BonusSub { get => SubWeaponType.BurstBomb; }
        public override SubWeaponBonusType BonusType { get => SubWeaponBonusType.Damage; }
        public override float ShotGravity => 0.02f;
        public override float AimDeviation { get => 0f; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToRangedWeapon(
                baseProjType: ModContent.ProjectileType<SplattershotProjectile>(),
                ammoID: AmmoID.None,
                singleShotTime: 5,
                shotVelocity: 9f);

            Item.damage = 18;
            Item.width = 60;
            Item.height = 32;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver();
        }
    }
}
