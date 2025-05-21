using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class RoyalHeavySplatling : HeavySplatling
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 28;
            Item.width = 92;
            Item.height = 50;
            Item.knockBack = 5;
            Item.SetValueMidHardmodeOre();
        }

        public override void AddRecipes() => AddRecipeOrichalcum();
    }
}
