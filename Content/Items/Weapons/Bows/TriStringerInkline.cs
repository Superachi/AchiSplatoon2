using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class TriStringerInkline : TriStringer
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 104;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TriStringer>());
            recipe.AddIngredient(ItemID.CobaltBar, 12);
            recipe.AddIngredient(ItemID.CrystalShard, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
