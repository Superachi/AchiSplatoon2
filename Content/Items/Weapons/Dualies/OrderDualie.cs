using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    [OrderWeapon]
    internal class OrderDualie : BaseDualie
    {
        public override float AimDeviation { get => 6f; }
        public override Vector2? HoldoutOffset() { return new Vector2(-10, 4); }
        public override Vector2 MuzzleOffset => new Vector2(50f, 0);

        // Dualie specific
        public override float PostRollDamageMod { get => 1.2f; }
        public override float PostRollAttackSpeedMod { get => 0.8f; }
        public override float PostRollAimMod { get => 0.5f; }
        public override float PostRollVelocityMod { get => 1.2f; }
        public override int MaxRolls { get => 1; }
        public override float RollDistance { get => 12f; }
        public override float RollDuration { get => 24f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<DualieShotProjectile>(),
                singleShotTime: 10,
                shotVelocity: 3.5f);

            Item.damage = 6;
            Item.width = 46;
            Item.height = 28;
            Item.knockBack = 0.5f;
            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;

            Item.ArmorPenetration = 3;
        }

        public override void AddRecipes() => AddRecipeOrder(ItemID.Emerald);
    }
}
