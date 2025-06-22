using AchiSplatoon2.ExtensionMethods;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class CarbonRoller : BaseRoller
    {
        public override float InkCost { get => 4f; }

        public override float GroundWindUpDelayModifier => 0.5f;
        public override float GroundAttackVelocityModifier => 0.85f;
        public override float JumpWindUpDelayModifier => 0.8f;
        public override float JumpAttackDamageModifier => 1.3f;
        public override float JumpAttackVelocityModifier => 1.2f;
        public override float RollingAccelModifier => 4f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 35;
            Item.knockBack = 3;

            Item.SetValuePostEvilBosses();
        }

        public override void AddRecipes() => AddRecipePostBee();
    }
}
