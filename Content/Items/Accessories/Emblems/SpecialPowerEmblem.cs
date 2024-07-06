using AchiSplatoon2.Content.Players;
using Terraria.ID;
using Terraria;
using Terraria.Localization;
using AchiSplatoon2.Helpers;

namespace AchiSplatoon2.Content.Items.Accessories.Emblems
{
    internal class SpecialPowerEmblem : BaseAccessory
    {
        public static float addValue = 1f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((int)(addValue * 100));
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
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var accMP = player.GetModPlayer<InkAccessoryPlayer>();
                if (accMP.hasAgentCloak) return;
                accMP.hasSpecialPowerEmblem = true;
                accMP.specialPowerMultiplier += addValue;
            }
        }
    }
}
