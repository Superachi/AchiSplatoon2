using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class ForgeSplattershotPro : SplattershotPro
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 42;
            Item.knockBack = 5;
            Item.SetValuePostMech();
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofFright);
    }
}
