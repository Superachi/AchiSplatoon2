using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class HeroShot : Splattershot
    {
        public override float ShotGravity { get => 0.4f; }
        public override int ShotGravityDelay => 10;
        public override int ShotExtraUpdates { get => 5; }
        public override float AimDeviation { get => 2f; }
        public override string ShootSample { get => "HeroShotShoot"; }
        public override Vector2? HoldoutOffset() { return new Vector2(-8, 0); }
        public override float MuzzleOffsetPx { get; set; } = 56f;
        public override float SubWeaponDamageBonus => 1f;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 60;
            Item.crit = 10;
            Item.shootSpeed = 8;
            Item.knockBack = 5f;

            Item.useTime = 5;
            Item.useAnimation = Item.useTime;

            Item.value = Item.buyPrice(gold: 50);
            Item.rare = ItemRarityID.Yellow;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.Anvils)
                .AddIngredient(ModContent.ItemType<SheldonLicenseGold>())
                .AddIngredient(ItemID.BrokenHeroSword)
                .AddIngredient(ItemID.ShroomiteBar, 10)
                .Register();
        }
    }
}
