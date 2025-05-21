using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class ZinkMiniSplatling : MiniSplatling
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 22;
            Item.knockBack = 4;
            Item.SetValueLowHardmodeOre();
        }

        public override void AddRecipes() => AddRecipeCobalt();
    }
}
