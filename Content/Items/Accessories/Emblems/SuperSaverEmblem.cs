using AchiSplatoon2.Content.Players;
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
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Pink;
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
                .AddIngredient(ItemID.SoulofSight, 1)
                .AddIngredient(ItemID.SoulofMight, 1)
                .AddIngredient(ItemID.SoulofFright, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
