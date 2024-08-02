using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class EnperrySplatDualie : SplatDualie
    {
        public override float PostRollVelocityMod { get => 1.2f; }
        public override float PostRollAimMod { get => 0.25f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<DualieShotProjectile>(),
                singleShotTime: 5,
                shotVelocity: 5f);

            Item.damage = 38;
            Item.width = 50;
            Item.height = 36;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofMight);
    }
}
