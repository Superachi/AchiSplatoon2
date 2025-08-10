using AchiSplatoon2.Attributes;
using AchiSplatoon2.ExtensionMethods;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    [ShimmerOrderWeapon]
    internal class ShimmerOrderRoller : OrderRoller
    {
        public override float InkCost { get => 0f; }

        public override float GroundWindUpDelayModifier => 1f;
        public override float GroundAttackVelocityModifier => 1f;
        public override float JumpWindUpDelayModifier => 1.5f;
        public override float JumpAttackDamageModifier => 1.3f;
        public override float JumpAttackVelocityModifier => 1.3f;
        public override float RollingAccelModifier => 2f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.SetValuePostEvilBosses();
        }

        public override void AddRecipes() { }
    }
}
