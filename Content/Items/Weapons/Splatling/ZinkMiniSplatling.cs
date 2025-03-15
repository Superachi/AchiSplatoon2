using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class ZinkMiniSplatling : MiniSplatling
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 22;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes() => AddRecipeCobalt();
    }
}
