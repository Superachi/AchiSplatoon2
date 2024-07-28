using AchiSplatoon2.Content.Projectiles.BrellaProjectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class SplatBrella : BaseBrella
    {
        public override int ProjectileCount { get => 5; }
        public override int ShieldLife => 100;
        public override int ShieldCooldown => 450;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BrellaShotgunProjectile>(),
                singleShotTime: 36,
                shotVelocity: 8f);

            Item.damage = 6;
            Item.width = 50;
            Item.height = 58;
            Item.knockBack = 2;

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseBasic(registerNow: false);
            recipe.AddIngredient(ItemID.MeteoriteBar, 5);
            recipe.Register();
        }
    }
}
