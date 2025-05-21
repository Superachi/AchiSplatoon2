using AchiSplatoon2.Attributes;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    [OrderWeapon]
    internal class OrderRoller : BaseRoller
    {
        public override float GroundWindUpDelayModifier => 1.5f;
        public override float GroundAttackVelocityModifier => 0.9f;
        public override float JumpWindUpDelayModifier => 2f;
        public override float JumpAttackDamageModifier => 1.3f;
        public override float JumpAttackVelocityModifier => 1.2f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 14;
            Item.knockBack = 4;

            Item.SetValuePreEvilBosses();
        }

        public override void AddRecipes() => AddRecipeOrder(ItemID.Ruby);
    }
}
