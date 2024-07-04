using AchiSplatoon2.Content.Projectiles.NozzlenoseProjectiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class H3Nozzlenose : L3Nozzlenose
    {
        public override int BurstShotTime { get => 5; }
        public override float DamageIncreasePerHit { get => 1.5f; }

        public override int ShotGravityDelay { get => 45; }
        public override float AimDeviation { get => 1f; }
        public override Vector2? HoldoutOffset() { return new Vector2(-2, 0); }
        public override float MuzzleOffsetPx { get; set; } = 48f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<NozzlenoseShooter>(),
                singleShotTime: 18,
                shotVelocity: 1f);

            Item.damage = 34;
            Item.width = 50;
            Item.height = 32;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.AdamantiteBar, 5);
            recipe.Register();
        }
    }
}
