using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    internal class ClashBlasterNeo : ClashBlaster
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 68;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipeOrichalcum();
    }
}
