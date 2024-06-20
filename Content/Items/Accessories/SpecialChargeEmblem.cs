using AchiSplatoon2.Content.Players;
using Terraria;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Items.Accessories
{
    internal class SpecialChargeEmblem : SpecialPowerEmblem
    {
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((int)((InkWeaponPlayer.specialChargeMultiplier - 1) * 100));
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();
            modPlayer.hasSpecialChargeEmblem = true;
        }
    }
}
