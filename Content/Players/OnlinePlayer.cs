using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class OnlinePlayer : ModPlayer
    {
        private int playerCountTimer = 15;
        private int playerCountTimerMax = 15;
        private int playerCount = 0;
        private bool playerCountChanged = false;

        public override void PreUpdate()
        {
            if (NetHelper.IsSinglePlayer()) return;

            playerCountChanged = false;

            if (playerCountTimer > 0) playerCountTimer--;
            if (playerCountTimer == 0)
            {
                playerCountTimer = playerCountTimerMax;

                var playerCountOld = playerCount;
                playerCount = 0;
                foreach (var p in Main.ActivePlayers)
                {
                    playerCount++;
                }

                playerCountChanged = playerCount != playerCountOld;
            }

            if (playerCountChanged)
            {
                DebugHelper.PrintInfo("Player count changed!");
                Player.GetModPlayer<WeaponPlayer>().SyncAllDataManual();
            }
        }
    }
}
