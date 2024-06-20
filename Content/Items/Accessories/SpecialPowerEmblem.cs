using AchiSplatoon2.Content.Players;
using Terraria.ID;
using Terraria;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Items.Accessories
{
    internal class SpecialPowerEmblem : BaseAccessory
    {
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((int)((InkWeaponPlayer.specialPowerMultiplier  -1) * 100));
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.buyPrice(gold: 2);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();
            modPlayer.hasSpecialPowerEmblem = true;
        }
    }
}
