using Terraria;

namespace AchiSplatoon2.Helpers;

internal static class PlayerHelper
{
    public static bool IsPlayerGrounded(Player player)
    {
        return player.velocity.Y == 0 && player.oldVelocity.Y == 0;
    }
}
