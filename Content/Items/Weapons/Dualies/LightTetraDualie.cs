using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class LightTetraDualie : DarkTetraDualie
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 32;
            Item.SetValuePostPlantera();
        }

        public override void AddRecipes()
        {
        }
    }
}
