using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class GoldDynamoRoller : DynamoRoller
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 140;
            Item.knockBack = 8;

            Item.SetValuePostMech();
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofFright);
    }
}
