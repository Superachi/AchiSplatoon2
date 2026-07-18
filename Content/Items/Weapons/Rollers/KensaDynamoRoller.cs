using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class KensaDynamoRoller : DynamoRoller
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 300;
            Item.knockBack = 8;

            Item.SetValuePostPlantera();
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
