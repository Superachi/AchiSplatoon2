using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class CarbonRollerDeco : CarbonRoller
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 70;
            Item.knockBack = 4;

            Item.SetValueLowHardmodeOre();
        }

        public override void AddRecipes() => AddRecipePalladium();
    }
}
