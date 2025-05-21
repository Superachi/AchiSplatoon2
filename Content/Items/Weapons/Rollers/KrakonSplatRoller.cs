using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class KrakonSplatRoller : SplatRoller
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 90;
            Item.knockBack = 6;

            Item.SetValueLowHardmodeOre();
        }

        public override void AddRecipes() => AddRecipeCobalt();
    }
}
