using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class ClassicSplattershotS1 : OctoShot
    {
        public override void AddRecipes()
        {
            Recipe recipe = CraftingReqs()
                .AddIngredient(ItemID.OrangeDye)
                .AddIngredient(ItemID.BlueDye)
                .Register();
        }
    }
}
