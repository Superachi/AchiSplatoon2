using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class OctoShot : Splattershot
    {
        public override float AimDeviation { get => 4f; }
        public override Vector2? HoldoutOffset() { return new Vector2(-8, 0); }
        public override Vector2 MuzzleOffset => new Vector2(50f, 0);

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.useTime = 6;
            Item.useAnimation = Item.useTime;
            Item.damage = 32;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        protected Recipe CraftingReqs()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(false)
                .AddIngredient(ModContent.ItemType<TentatekSplattershot>())
                .AddIngredient(ItemID.HallowedBar, 8);

            return recipe;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CraftingReqs()
                .Register();
        }
    }
}
