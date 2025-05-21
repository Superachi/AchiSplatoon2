using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class KensaSplatRoller : SplatRoller
    {
        public override float GroundAttackVelocityModifier => 1.5f;
        public override float JumpAttackVelocityModifier => 2f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 140;
            Item.knockBack = 6;

            Item.SetValuePostPlantera();
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
