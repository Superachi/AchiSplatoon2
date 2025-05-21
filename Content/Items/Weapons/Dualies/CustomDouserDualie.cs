using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class CustomDouserDualie : DouserDualie
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 38;
            Item.SetValuePostPlantera();
        }

        public override void AddRecipes()
        {
        }
    }
}
