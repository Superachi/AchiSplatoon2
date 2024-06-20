using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class Splattershot : BaseSplattershot
    {
        public override SubWeaponType BonusSub { get => SubWeaponType.SplatBomb; }
        public override SubWeaponBonusType BonusType { get => SubWeaponBonusType.Discount; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToRangedWeapon(
                baseProjType: ModContent.ProjectileType<SplattershotProjectile>(),
                ammoID: AmmoID.None,
                singleShotTime: 8,
                shotVelocity: 9f);

            Item.damage = 14;
            Item.width = 42;
            Item.height = 26;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseBasic(registerNow: false);
            recipe.AddIngredient(ItemID.DemoniteBar, 5);
            recipe.Register();
        }
    }
}
