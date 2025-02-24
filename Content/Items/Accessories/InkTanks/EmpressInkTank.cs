using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Items.Accessories.InkTanks
{
    internal class EmpressInkTank : InkTank
    {
        public override int CapacityBonus => 50;
        public static int ProjectileDamage => 40;
        public static int ProcCooldown => 9;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CapacityBonus);

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Cyan;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);

            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var accessoryPlayer = player.GetModPlayer<AccessoryPlayer>();
                accessoryPlayer.hasThermalInkTank = true;
                accessoryPlayer.TryEquipAccessory<EmpressInkTank>();
            }
        }
    }
}
