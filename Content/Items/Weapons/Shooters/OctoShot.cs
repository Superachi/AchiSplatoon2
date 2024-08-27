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
        public override float MuzzleOffsetPx { get; set; } = 50f;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.useTime = 6;
            Item.useAnimation = Item.useTime;
            Item.damage = 42;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.LightPurple;
        }

        protected Recipe CraftingReqs() => AddRecipePostMechBoss(false, ItemID.SoulofFright);

        public override void AddRecipes()
        {
            Recipe recipe = CraftingReqs()
                .AddIngredient(ItemID.BlackDye)
                .Register();
        }
    }
}
