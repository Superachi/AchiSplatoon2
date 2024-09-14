using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class TriSlosherNouveau : TriSlosher
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 76;
            Item.knockBack = 7;

            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofMight);
    }
}
