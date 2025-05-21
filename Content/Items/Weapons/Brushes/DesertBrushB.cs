using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class DesertBrushB : DesertBrush
    {
        public override float InkCost { get => 2f; }
        public override float BaseWeaponUseTime => 12f;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 30;
            Item.knockBack = 1;

            Item.SetValuePostMech();
        }

        public override void AddRecipes() => AddRecipeChlorophyteUpgrade(true, ModContent.ItemType<DesertBrush>());
    }
}
