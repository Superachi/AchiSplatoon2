using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class Slosher : BaseSlosher
    {
        public override int ShotCount => 10;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 20;
            Item.width = 32;
            Item.height = 32;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes() => AddRecipeMeteorite();
    }
}
