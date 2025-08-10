using AchiSplatoon2.Attributes;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    [ShimmerOrderWeapon]
    internal class ShimmerOrderShot : OrderShot
    {
        public override float InkCost { get => 0f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.useTime = 7;
            Item.useAnimation = Item.useTime;

            Item.SetValuePostEvilBosses();
        }

        public override void AddRecipes() { }
    }
}
