using AchiSplatoon2.Content.Projectiles.NozzlenoseProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class KensaL3Nozzlenose : L3Nozzlenose
    {
        public override int BurstShotTime { get => 3; }
        public override float DamageIncreasePerHit { get => 1f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<NozzlenoseShooter>(),
                singleShotTime: 14,
                shotVelocity: 1f);

            Item.damage = 40;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
