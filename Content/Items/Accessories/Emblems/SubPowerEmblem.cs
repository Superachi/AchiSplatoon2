using AchiSplatoon2.Content.Players;
using Terraria;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Items.Accessories.Emblems
{
    internal class SubPowerEmblem : SpecialPowerEmblem
    {
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((int)((InkAccessoryPlayer.subPowerMultiplier - 1) * 100));

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<InkAccessoryPlayer>();
            modPlayer.hasSubPowerEmblem = true;
        }
    }
}
