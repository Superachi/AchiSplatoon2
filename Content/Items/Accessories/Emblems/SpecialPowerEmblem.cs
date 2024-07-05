using AchiSplatoon2.Content.Players;
using Terraria.ID;
using Terraria;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Items.Accessories.Emblems
{
    internal class SpecialPowerEmblem : BaseAccessory
    {
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((int)((InkAccessoryPlayer.specialPowerMultiplier - 1) * 100));
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 28;
            Item.value = Item.buyPrice(gold: 2);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<InkAccessoryPlayer>();
            modPlayer.hasSpecialPowerEmblem = true;
        }
    }
}
