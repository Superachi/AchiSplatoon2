using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    internal class FreshSquiffer : ClassicSquiffer
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 120;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes() => AddRecipeCobalt();
    }
}
