using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;

namespace AchiSplatoon2.Content.Items.Consumables.DroneUpgradeDiscs
{
    internal class DroneDiscDowngrade : DroneDiscBase
    {
        public override bool CanUseItem(Player player)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                return true;
            }

            return false;
        }

        public override bool? UseItem(Player player)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<PearlDronePlayer>();
                modPlayer.ResetLevel();
                return true;
            }

            return false;
        }
    }
}
