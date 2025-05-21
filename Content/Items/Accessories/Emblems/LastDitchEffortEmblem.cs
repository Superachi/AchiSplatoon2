using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Items.Accessories.Emblems
{
    [ItemCategory("Accessory", "Emblems")]
    internal class LastDitchEffortEmblem : BaseAccessory
    {
        public static float LifePercentageThreshold = 0.6f;
        public static float InkSaverAmount = 0.5f;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(
            (int)Math.Round(LifePercentageThreshold * 100),
            (int)Math.Round(InkSaverAmount * 100));

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 28;
            Item.SetValueMidHardmodeOre();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<LastDitchEffortEmblem>();
        }
    }
}
