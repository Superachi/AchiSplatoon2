using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class DesertBrushB : DesertBrush
    {
        public override float InkCost { get => 4f; }
        public override float BaseWeaponUseTime => 18f;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 44;
            Item.knockBack = 6;

            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipeChlorophyteUpgrade(true, ModContent.ItemType<DesertBrush>());
    }
}
