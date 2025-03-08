using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class TriSlosherNouveau : TriSlosher
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 80;
            Item.knockBack = 7;

            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofMight);
    }
}
