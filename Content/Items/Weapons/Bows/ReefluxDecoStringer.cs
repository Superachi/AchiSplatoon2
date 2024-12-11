using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class ReefluxDecoStringer : ReefluxStringer
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 50;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes() => AddRecipeOrichalcum();
    }
}
