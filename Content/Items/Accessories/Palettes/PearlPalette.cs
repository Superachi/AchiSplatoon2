using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Accessories.Palettes
{
    internal class PearlPalette : ChipPalette
    {
        protected override int PaletteCapacity { get => 8; }
        protected override MainWeaponStyle WeaponStyle => MainWeaponStyle.Dualies;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 32;
            Item.height = 28;
            Item.SetValueHighHardmodeOre();
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            AddRecipeWithSheldonLicenseSilver(registerNow: false)
                .Register();
        }
    }
}
