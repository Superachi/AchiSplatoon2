using AchiSplatoon2.Attributes;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    [ShimmerOrderWeapon]
    internal class ShimmerOrderBlaster : OrderBlaster
    {
        public override float InkCost { get => 0f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.SetValuePostEvilBosses();
        }

        public override void AddRecipes() { }
    }
}
