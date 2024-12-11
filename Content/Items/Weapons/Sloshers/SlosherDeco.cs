using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class SlosherDeco : Slosher
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 70;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes() => AddRecipeMythril();
    }
}
