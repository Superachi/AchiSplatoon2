using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class KensaDynamoRoller : DynamoRoller
    {
        public override float GroundWindUpDelayModifier => 1.8f;
        public override float JumpWindUpDelayModifier => 2.5f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 200;
            Item.knockBack = 8;

            Item.SetValuePostPlantera();
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
