using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    internal class CustomEliterCharger : EliterCharger
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 300;
            Item.knockBack = 8;
            Item.SetValuePostMech();
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofSight);
    }
}
