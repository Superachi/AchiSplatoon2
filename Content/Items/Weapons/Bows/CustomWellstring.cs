using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class CustomWellstring : Wellstring
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 150;

            Item.SetValuePostPlantera();
        }

        public override void AddRecipes()
        {
        }
    }
}
