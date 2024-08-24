using AchiSplatoon2.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Items.CraftingMaterials;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class SlimeSplattershot : Splattershot
    {
        public override string ShootSample => "Dot52GalShoot";
        public override float ShotGravity { get => 0.4f; }
        public override int ShotGravityDelay { get => 15; }
        public override int ShotExtraUpdates { get => 5; }
        public override float AimDeviation { get => 2f; }
        public override Vector2? HoldoutOffset() { return new Vector2(-8, 0); }
        public override float MuzzleOffsetPx { get; set; } = 44f;

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SplattershotProjectile>(),
                singleShotTime: 24,
                shotVelocity: 7f);

            Item.damage = 18;
            Item.width = 42;
            Item.height = 26;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<OrderShot>())
                .AddIngredient(ItemID.SlimeGun)
                .Register();
        }
    }
}
