using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class SlosherDeco : Slosher
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 72;
            Item.knockBack = 8;
            Item.SetValueMidHardmodeOre();
        }

        public override void AddRecipes() => AddRecipeMythril();
    }
}
