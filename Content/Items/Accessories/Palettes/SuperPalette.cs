using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Items.Weapons;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.Palettes
{
    internal class SuperPalette : ChipPalette
    {
        protected override int PaletteCapacity { get => 16; }
        protected override MainWeaponStyle WeaponStyle { get => MainWeaponStyle.Other; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SuperPaletteLeftPart>(), 1)
                .AddIngredient(ModContent.ItemType<SuperPaletteRightPart>(), 1)
                .AddIngredient(ModContent.ItemType<SuperPaletteMiddlePart>(), 1)
                .Register();
        }
    }
}
