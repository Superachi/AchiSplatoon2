using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    internal class ZFSplatCharger : SplatCharger
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 170;
            Item.knockBack = 5;
            Item.SetValueMidHardmodeOre();
        }

        public override void AddRecipes() => AddRecipeOrichalcum();
    }
}
