using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Helpers;

internal static class PlayerHelper
{
    public static bool IsPlayerGrounded(Player player)
    {
        return player.position.Y == player.oldPosition.Y && Collision.SolidCollision(player.Bottom, 8, 8) && player.velocity.Y == 0;
    }

    public static bool IsPlayerOntopOfTile(Player player)
    {
        var tileUnderPlayer = Framing.GetTileSafely((player.Bottom + Vector2.UnitY).ToTileCoordinates());
        return tileUnderPlayer.TileType != 0;
    }

    public static bool IsPlayerOntopOfPlatform(Player player)
    {
        var tileUnderPlayer = Framing.GetTileSafely((player.Bottom + Vector2.UnitY).ToTileCoordinates());
        return Main.tileSolidTop[tileUnderPlayer.TileType];
    }
}
