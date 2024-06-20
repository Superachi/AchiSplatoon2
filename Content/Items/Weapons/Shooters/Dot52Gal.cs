using AchiSplatoon2.Content.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class Dot52Gal : BaseSplattershot
    {
        public override string ShootSample { get => "Dot52GalShoot"; }
        public override float MuzzleOffsetPx { get; set; } = 48f;
        public override Vector2? HoldoutOffset() { return new Vector2(-2, 0); }

        public override SubWeaponType BonusSub { get => SubWeaponType.Sprinkler; }
        public override SubWeaponBonusType BonusType { get => SubWeaponBonusType.Discount; }
        public override float AimDeviation { get => 8f; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToRangedWeapon(
                baseProjType: ModContent.ProjectileType<SplattershotProjectile>(),
                ammoID: AmmoID.None,
                singleShotTime: 9,
                shotVelocity: 8f);

            Item.damage = 52;
            Item.width = 52;
            Item.height = 30;
            Item.knockBack = 4;
            Item.crit = 8;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.CursedFlame, 10);
            recipe.Register();
        }
    }
}
