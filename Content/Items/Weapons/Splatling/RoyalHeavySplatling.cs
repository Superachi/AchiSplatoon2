using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class RoyalHeavySplatling : HeavySplatling
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 32;
            Item.width = 92;
            Item.height = 50;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes() => AddRecipeOrichalcum();
    }
}
