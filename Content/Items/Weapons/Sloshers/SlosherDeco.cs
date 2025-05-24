using AchiSplatoon2.ExtensionMethods;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class SlosherDeco : Slosher
    {
        public override int ShotCount => 10;

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
