using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.InkTanks
{
    internal class IchorInkTank : InkTank
    {
        public static int DebuffChanceDenominator => 10;
        public static int DebuffDuration => 120;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CapacityBonus);

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.SetValueHighHardmodeOre();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);

            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var accessoryPlayer = player.GetModPlayer<AccessoryPlayer>();
                accessoryPlayer.hasThermalInkTank = true;
                accessoryPlayer.TryEquipAccessory<IchorInkTank>();
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.WorkBenches)
                .AddIngredient(ModContent.ItemType<InkTank>())
                .AddIngredient(ItemID.SoulofNight, 8)
                .AddIngredient(ItemID.Ichor, 12)
                .AddIngredient(ItemID.DarkShard, 1)
                .Register();
        }
    }
}
