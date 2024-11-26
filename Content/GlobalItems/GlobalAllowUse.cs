using AchiSplatoon2.Content.Players;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.GlobalItems
{
    internal class GlobalAllowUse : GlobalItem
    {
        private SquidPlayer _squidPlayer => Main.LocalPlayer.GetModPlayer<SquidPlayer>();

        public override bool CanUseItem(Item item, Player player)
        {
            if (_squidPlayer.IsSquid())
            {
                return false;
            }

            return base.CanUseItem(item, player);
        }
    }
}
