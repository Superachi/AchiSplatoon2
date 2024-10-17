using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class SplatRoller : BaseRoller
    {
        public override float JumpWindUpDelayModifier => 1.5f;
        public override float JumpAttackDamageModifier => 1.3f;
        public override float JumpAttackVelocityModifier => 1.5f;
        public override float RollingAccelModifier => 2f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 28;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes() => AddRecipePostEOC();
    }
}
