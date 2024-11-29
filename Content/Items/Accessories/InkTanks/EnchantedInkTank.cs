using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.InkTanks
{
    internal class EnchantedInkTank : InkTank
    {
        public override int CapacityBonus => 40;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.WorkBenches)
                .AddIngredient(ModContent.ItemType<InkTank>())
                .AddIngredient(ItemID.FallenStar, 5)
                .AddIngredient(ItemID.DemoniteBar, 5)
                .Register();

            CreateRecipe()
                .AddTile(TileID.WorkBenches)
                .AddIngredient(ModContent.ItemType<InkTank>())
                .AddIngredient(ItemID.FallenStar, 5)
                .AddIngredient(ItemID.CrimtaneBar, 5)
                .Register();
        }
    }
}
