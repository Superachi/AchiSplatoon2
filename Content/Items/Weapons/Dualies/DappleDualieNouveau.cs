using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class DappleDualieNouveau : DappleDualie
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 24;
            Item.shootSpeed = 3.5f;
            Item.crit = 5;
            Item.knockBack = 3;
            Item.SetValueMidHardmodeOre();
        }

        public override void AddRecipes() => AddRecipeOrichalcum();
    }
}
