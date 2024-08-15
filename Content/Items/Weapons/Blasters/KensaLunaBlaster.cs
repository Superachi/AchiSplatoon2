using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Projectiles.BlasterProjectiles;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    internal class KensaLunaBlaster : LunaBlaster
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BlasterProjectileV2>(),
                singleShotTime: 30,
                shotVelocity: 6f);

            Item.damage = 180;
            Item.width = 42;
            Item.height = 44;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
