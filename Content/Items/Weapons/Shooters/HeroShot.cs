using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class HeroShot : Splattershot
    {
        public override float ShotGravity { get => 0.4f; }
        public override int ShotGravityDelay => 10;
        public override int ShotExtraUpdates { get => 5; }
        public override float AimDeviation { get => 2f; }
        public override SoundStyle ShootSample { get => SoundPaths.HeroShotShoot.ToSoundStyle(); }
        public override Vector2? HoldoutOffset() { return new Vector2(-8, 0); }
        public override Vector2 MuzzleOffset => new Vector2(56f, 0);

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 45;
            Item.crit = 10;
            Item.shootSpeed = 8;
            Item.knockBack = 5f;

            Item.useTime = 5;
            Item.useAnimation = Item.useTime;

            Item.SetValueEndgame();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.Anvils)
                .AddIngredient(ModContent.ItemType<SheldonLicenseGold>())
                .AddIngredient(ModContent.ItemType<OctoShot>())
                .AddIngredient(ItemID.BrokenHeroSword)
                .Register();

            CreateRecipe()
                .AddTile(TileID.Anvils)
                .AddIngredient(ModContent.ItemType<SheldonLicenseGold>())
                .AddIngredient(ModContent.ItemType<ClassicSplattershotS1>())
                .AddIngredient(ItemID.BrokenHeroSword)
                .Register();

            CreateRecipe()
                .AddTile(TileID.Anvils)
                .AddIngredient(ModContent.ItemType<SheldonLicenseGold>())
                .AddIngredient(ModContent.ItemType<ClassicSplattershotS2>())
                .AddIngredient(ItemID.BrokenHeroSword)
                .Register();
        }
    }
}
