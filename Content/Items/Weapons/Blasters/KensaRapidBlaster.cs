using Terraria.ID;
using Terraria;
using AchiSplatoon2.Content.Projectiles;
using Terraria.ModLoader;

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

            Item.damage = 150;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
