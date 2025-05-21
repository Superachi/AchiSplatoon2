using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    internal class FreshSquiffer : ClassicSquiffer
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 130;
            Item.SetValueLowHardmodeOre();
        }

        public override void AddRecipes() => AddRecipeCobalt();
    }
}
