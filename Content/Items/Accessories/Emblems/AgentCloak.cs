using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.Emblems
{
    [ItemCategory("Accessory", "Emblems")]
    internal class AgentCloak : BaseAccessory
    {
        public static float specialChargeMultiplier = 0.5f;
        public static float subPowerMultiplier = 1f;
        public static float specialPowerMultiplier = 1f;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(
            (int)(subPowerMultiplier * 100),
            (int)(specialPowerMultiplier * 100),
            (int)(specialChargeMultiplier * 100)
            );

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 26;
            Item.height = 32;
            Item.SetValueHighHardmodeOre();
        }

        public static void ApplyStats(Player player)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var accMP = player.GetModPlayer<AccessoryPlayer>();
                accMP.specialChargeMultiplier += specialChargeMultiplier;
                accMP.subPowerMultiplier += subPowerMultiplier;
                accMP.specialPowerMultiplier += specialPowerMultiplier;
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ApplyStats(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 5)
                .AddIngredient(ModContent.ItemType<InkDroplet>(), 10)
                .AddIngredient(ModContent.ItemType<SpecialPowerEmblem>(), 1)
                .AddIngredient(ItemID.SoulofSight, 1)
                .AddIngredient(ItemID.SoulofMight, 1)
                .AddIngredient(ItemID.SoulofFright, 1)
                .AddTile(TileID.Loom)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.Silk, 5)
                .AddIngredient(ModContent.ItemType<InkDroplet>(), 10)
                .AddIngredient(ModContent.ItemType<SubPowerEmblem>(), 1)
                .AddIngredient(ItemID.SoulofSight, 1)
                .AddIngredient(ItemID.SoulofMight, 1)
                .AddIngredient(ItemID.SoulofFright, 1)
                .AddTile(TileID.Loom)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.Silk, 5)
                .AddIngredient(ModContent.ItemType<InkDroplet>(), 10)
                .AddIngredient(ModContent.ItemType<SpecialChargeEmblem>(), 1)
                .AddIngredient(ItemID.SoulofSight, 1)
                .AddIngredient(ItemID.SoulofMight, 1)
                .AddIngredient(ItemID.SoulofFright, 1)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}
