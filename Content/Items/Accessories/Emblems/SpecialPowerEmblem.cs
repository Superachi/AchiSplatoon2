using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Items.Accessories.Emblems
{
    [ItemCategory("Accessory", "Emblems")]
    internal class SpecialPowerEmblem : BaseAccessory
    {
        public static float addValue = 1f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((int)(addValue * 100));
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 28;
            Item.SetValueLowHardmodeOre();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var accMP = player.GetModPlayer<AccessoryPlayer>();
                accMP.hasSpecialPowerEmblem = true;
                accMP.specialPowerMultiplier += addValue;
            }
        }
    }
}
