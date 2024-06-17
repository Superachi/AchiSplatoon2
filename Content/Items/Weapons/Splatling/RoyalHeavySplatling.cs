using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class RoyalHeavySplatling : HeavySplatling
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 48;
            Item.width = 92;
            Item.height = 50;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<HeavySplatling>());
            recipe.AddIngredient(ItemID.OrichalcumBar, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
