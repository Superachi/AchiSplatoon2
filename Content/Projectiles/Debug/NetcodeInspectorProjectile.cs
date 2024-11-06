using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.Debug
{
    internal class NetcodeInspectorProjectile : BaseProjectile
    {
        private int pingTimer = 0;
        private int pingTimerMax = 30;
        private int pingMs = 0;
        private DateTime timeSincePingSent = DateTime.Now;
        private DateTime timeSincePingReceived = DateTime.Now;

        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.timeLeft = 1_000_000;
        }

        public override void AfterSpawn()
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                DebugHelper.PrintWarning("Netcode Inspector doesn't work in single player.");
                Projectile.Kill();
                return;
            }

            DebugHelper.PrintInfo("Activated Netcode Inspector");
        }

        public override void AI()
        {
            Projectile.Center = GetOwner().Center;
            if (!IsThisClientTheProjectileOwner()) return;

            pingTimer--;
            if (pingTimer <= 0)
            {
                pingTimer = pingTimerMax;
                timeSincePingSent = DateTime.Now;
                NetHelper.SendPingToServer();
            }
        }

        private Vector2 GetPlayerDrawPosition(Player player, float xOffset = 0, float yOffset = 0)
        {
            var c = player.Center;
            return new Vector2((int)c.X, (int)c.Y) - Main.screenPosition + new Vector2(0 + xOffset, player.gfxOffY + yOffset);
        }

        public override void PostDraw(Color lightColor)
        {
            if (!IsThisClientTheProjectileOwner()) return;

            foreach (var player in Main.ActivePlayers)
            {
                if (player.whoAmI != GetOwner().whoAmI)
                {
                    var colorChipPlayer = player.GetModPlayer<ColorChipPlayer>();
                    string allyText = $"Player {player.whoAmI} ({player.name})";

                    Utils.DrawBorderString(
                        Main.spriteBatch, $"{allyText}",
                        GetPlayerDrawPosition(player, 0, 60),
                        colorChipPlayer.GetColorFromChips(),
                        anchorx: 0.5f);
                }
            }

            string playerText = $"Ping ms: {pingMs}\n(last received: {timeSincePingReceived.ToString("HH:mm `ss:fff")})";

            Utils.DrawBorderString(
                Main.spriteBatch, $"{playerText}",
                GetPlayerDrawPosition(GetOwner(), 0, 60),
                Color.White,
                anchorx: 0.5f);
        }

        public void ReceiveServerPing()
        {
            timeSincePingReceived = DateTime.Now;
            pingMs = (timeSincePingReceived - timeSincePingSent).Milliseconds;
        }
    }
}
