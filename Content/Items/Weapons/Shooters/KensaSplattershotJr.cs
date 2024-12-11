using AchiSplatoon2.Content.Projectiles.ShooterProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class KensaSplattershotJr : SplattershotJr
    {
        public override float InkTankCapacityBonus { get => 60f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SplattershotProjectile>(),
                singleShotTime: 5,
                shotVelocity: 5f);

            Item.damage = 36;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes() => AddRecipeKensa();
    }
}
