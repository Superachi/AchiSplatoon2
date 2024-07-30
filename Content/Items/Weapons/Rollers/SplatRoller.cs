using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class SplatRoller : BaseRoller
    {
        public override float JumpWindUpDelayModifier => 1.5f;
        public override float JumpAttackDamageModifier => 1.5f;
        public override float JumpAttackVelocityModifier => 1.5f;
        public override float RollingAccelModifier => 2f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 20;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseBasic(registerNow: false);
            recipe.AddIngredient(ItemID.DemoniteBar, 5);
            recipe.Register();

            var altRecipe = AddRecipeWithSheldonLicenseBasic(registerNow: false);
            altRecipe.AddIngredient(ItemID.CrimtaneBar, 5);
            altRecipe.Register();
        }
    }
}
