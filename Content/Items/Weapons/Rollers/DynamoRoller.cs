using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class DynamoRoller : BaseRoller
    {
        public override float InkCost { get => 24f; }

        public override float GroundWindUpDelayModifier => 2.1f;
        public override float GroundAttackVelocityModifier => 1.3f;
        public override float JumpWindUpDelayModifier => 2.8f;
        public override float JumpAttackDamageModifier => 1.3f;
        public override float JumpAttackVelocityModifier => 1.6f;
        public override float RollingAccelModifier => 1f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 90;
            Item.knockBack = 7;

            Item.SetValueLatePreHardmode();
        }

        public override void AddRecipes() => AddRecipePostSkeletron();
    }
}
