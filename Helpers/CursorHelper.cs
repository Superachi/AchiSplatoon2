using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Players;
using Terraria;

namespace AchiSplatoon2.Helpers
{
    public static class CursorHelper
    {
        public static bool CursorHasInteractable()
        {
            var player = Main.LocalPlayer;

            if (Main.HoveringOverAnNPC)
            {
                // DebugHelper.PrintDebug($"{nameof(Main.HoveringOverAnNPC)}: {Main.HoveringOverAnNPC}");
                return true;
            }

            if (player.mouseInterface)
            {
                // DebugHelper.PrintDebug($"{nameof(player.mouseInterface)}: {player.mouseInterface}");
                return true;
            }

            // Credit to 'cat' on the tmodloader discord for finding this solution: https://discord.com/channels/103110554649894912/534215632795729922/1266721389075759104
            if (player.GetModPlayer<CursorPlayer>().cursorHoveredLastFrame)
            {
                // DebugHelper.PrintDebug($"cursorHoveredLastFrame: {player.GetModPlayer<CursorPlayer>().cursorHoveredLastFrame}");
                return true;
            }

            if (Main.SmartInteractTileCoords.Count != 0)
            {
                // DebugHelper.PrintDebug($"{nameof(Main.SmartInteractTileCoords)}: {Main.SmartInteractTileCoords}");
                return true;
            }

            if (Main.mouseItem.stack > 0
                && (Main.mouseItem.ModItem is not BaseWeapon && player.HeldItem.ModItem is not BaseWeapon))
            {
                // DebugHelper.PrintDebug($"{nameof(Main.mouseItem.stack)}: {Main.mouseItem.stack}");
                return true;
            }

            return false;
        }
    }
}
