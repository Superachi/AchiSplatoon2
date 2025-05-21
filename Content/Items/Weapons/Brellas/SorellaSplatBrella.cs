using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class SorellaSplatBrella : SplatBrella
    {
        public override int ProjectileCount { get => 6; }
        public override int ShieldLife => 350;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 24;
            Item.knockBack = 3;

            Item.SetValueHighHardmodeOre();
        }

        public override void AddRecipes() => AddRecipeTitanium();
    }
}
