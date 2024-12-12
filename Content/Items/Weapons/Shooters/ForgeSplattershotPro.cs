using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class ForgeSplattershotPro : SplattershotPro
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 38;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofFright);
    }
}
