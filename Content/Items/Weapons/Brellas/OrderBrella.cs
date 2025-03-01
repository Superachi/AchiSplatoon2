using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Projectiles.BrellaProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    [OrderWeapon]
    internal class OrderBrella : BaseBrella
    {
        public override int ProjectileCount { get => 3; }
        public override int ShieldLife => 80;
        public override int ShieldCooldown => 450;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BrellaShotgunProjectile>(),
                singleShotTime: 40,
                shotVelocity: 8f);

            Item.damage = 8;
            Item.width = 50;
            Item.height = 58;
            Item.knockBack = 1;

            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;

            // Note: hide this stat from the player-- the Order Brella shouldn't be seen as a swapout for high-def enemies
            Item.ArmorPenetration = 3;
        }

        public override void AddRecipes() => AddRecipeOrder(ItemID.Diamond);
    }
}
