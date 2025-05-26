using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.Emblems
{
    internal class SuperSaverEmblem : MainSaverEmblem
    {
        public override LocalizedText Tooltip => GetAssociatedTooltip().WithFormatArgs(
            (int)Math.Round(InkSaverAmount() * 100),
            (int)Math.Round(InkSaverAmount() * 100)
            );

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 28;
            Item.SetValueHighHardmodeOre();
        }

        public override float InkSaverAmount()
        {
            return 0.3f;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<SuperSaverEmblem>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MainSaverEmblem>())
                .AddIngredient(ModContent.ItemType<SubSaverEmblem>())
                .AddIngredient(ModContent.ItemType<LastDitchEffortEmblem>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
