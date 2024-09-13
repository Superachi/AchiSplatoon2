using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Projectiles.BlasterProjectiles;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    internal class KensaRapidBlaster : RapidBlaster
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BlasterProjectileV2>(),
                singleShotTime: 25,
                shotVelocity: 11f);

            Item.damage = 120;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
