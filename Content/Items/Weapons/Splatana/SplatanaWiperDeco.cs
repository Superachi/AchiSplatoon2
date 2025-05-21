using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class SplatanaWiperDeco : SplatanaWiper
    {
        public override int BaseDamage { get => 25; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = DisplayDamage(BaseDamage);

            Item.SetValueLowHardmodeOre();
        }

        public override void AddRecipes() => AddRecipePalladium();
    }
}
