using AchiSplatoon2.Content.Players;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.Emblems
{
    internal class SquidbeakCloak : BaseAccessory
    {
        public override LocalizedText Tooltip => GetAssociatedTooltip().WithFormatArgs(
            (int)(AgentCloak.subPowerMultiplier * 100),
            (int)(AgentCloak.specialPowerMultiplier * 100),
            (int)(AgentCloak.specialChargeMultiplier * 100),
            (int)Math.Round(new SuperSaverEmblem().InkSaverAmount() * 100),
            (int)Math.Round(new SuperSaverEmblem().InkSaverAmount() * 100)
            );

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 34;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.Lime;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<SquidbeakCloak>();
            AgentCloak.ApplyStats(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AgentCloak>())
                .AddIngredient(ModContent.ItemType<SuperSaverEmblem>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
