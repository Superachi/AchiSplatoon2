using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class ClassicSplattershotS2 : OctoShot
    {
        public override void AddRecipes()
        {
            Recipe recipe = CraftingReqs()
                .AddIngredient(ItemID.PinkDye)
                .AddIngredient(ItemID.LimeDye)
                .Register();
        }
    }
}
