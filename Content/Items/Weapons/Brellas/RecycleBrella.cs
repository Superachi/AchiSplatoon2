using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Projectiles.BrellaProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class RecycleBrella : BaseBrella
    {
        public override float ShotGravity { get => 0.4f; }
        public override int ShotGravityDelay { get => 32; }
        public override float AimDeviation { get => 0f; }

        // Brella specific
        public override int ProjectileCount { get => 5; }
        public override float ShotgunArc { get => 4f; }
        public override float ShotVelocityRandomRange => 0.05f;
        public override int ShieldLife => 150;
        public override int ShieldCooldown => 180;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BrellaShotgunProjectile>(),
                singleShotTime: 36,
                shotVelocity: 10f);

            Item.damage = 24;
            Item.width = 50;
            Item.height = 58;
            Item.knockBack = 4;

            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ModContent.ItemType<BambooMk1Charger>());
            recipe.AddIngredient(ModContent.ItemType<SplatBrella>());
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.Register();
        }
    }
}
