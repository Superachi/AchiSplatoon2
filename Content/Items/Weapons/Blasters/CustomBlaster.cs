using AchiSplatoon2.Content.Projectiles.BlasterProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    internal class CustomBlaster : Blaster
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BlasterProjectile>(),
                singleShotTime: 50,
                shotVelocity: 7f);

            // Matches Range Blaster's damage, but is obtained later and has less range.
            Item.damage = 160;
            Item.knockBack = 5;

            Item.SetValuePostMech();
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofFright);
    }
}
