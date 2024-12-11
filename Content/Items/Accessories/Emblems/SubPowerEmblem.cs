using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Items.Accessories.Emblems
{
    internal class SubPowerEmblem : SpecialPowerEmblem
    {
        public new static float addValue = 1f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((int)(addValue * 100));

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var accMP = player.GetModPlayer<AccessoryPlayer>();
                if (accMP.hasAgentCloak) return;
                accMP.hasSubPowerEmblem = true;
                accMP.subPowerMultiplier += addValue;
            }
        }
    }
}
