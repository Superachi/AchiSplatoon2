using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class CorruptionBrellaB : CorruptionBrella
    {
        public override int ShieldLife => 400;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 40;
            Item.knockBack = 9;

            Item.SetValuePostMech();
        }

        public override void AddRecipes() => AddRecipeChlorophyteUpgrade(true, ModContent.ItemType<CorruptionBrella>());
    }
}
