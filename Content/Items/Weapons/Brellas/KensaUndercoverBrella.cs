using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class KensaUndercoverBrella : UndercoverBrella
    {
        public override int ShieldLife => 300;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 38;
            Item.knockBack = 3;

            Item.SetValuePostPlantera();
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
