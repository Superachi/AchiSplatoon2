using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class SplatRoller : BaseRoller
    {
        public override float JumpWindUpDelayModifier => 1.5f;
        public override float JumpAttackDamageModifier => 1.3f;
        public override float JumpAttackVelocityModifier => 1.4f;
        public override float RollingAccelModifier => 2f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 28;
            Item.knockBack = 5;
            Item.SetValuePreEvilBosses();
        }

        public override void AddRecipes() => AddRecipePostEOC();
    }
}
