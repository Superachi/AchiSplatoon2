using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    internal class ClashBlasterNeo : ClashBlaster
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 68;
            Item.SetValueMidHardmodeOre();
        }

        public override void AddRecipes() => AddRecipeOrichalcum();
    }
}
