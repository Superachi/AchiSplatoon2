using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class Inkbrush : BaseBrush
    {
        protected override int ArmorPierce => 5;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 9;
            Item.knockBack = 3;

            Item.scale = 1;
            Item.useTime = 12;
            Item.useAnimation = Item.useTime;

            Item.width = 56;
            Item.height = 64;

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes() => AddRecipeMeteorite();
    }
}
