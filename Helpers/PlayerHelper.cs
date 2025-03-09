using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.IO;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Helpers;

internal static class PlayerHelper
{
    public static bool IsPlayerGrounded(Player player)
    {
        var width = 32;
        bool isOnBlock = Collision.SolidCollision(player.Bottom + new Vector2(-width / 2, 0), width, 8) || IsPlayerOntopOfPlatform(player);
        return player.position.Y == player.oldPosition.Y && isOnBlock && player.velocity.Y == 0;
    }

    public static bool IsPlayerGrappled(Player player)
    {
        bool isHooked = false;
        foreach(var proj in Main.ActiveProjectiles)
        {
            if (proj.owner == player.whoAmI && proj.aiStyle == ProjAIStyleID.Hook)
            {
                isHooked = true;
                break;
            }
        }

        bool isStill = Math.Abs(player.velocity.X) < 1 || Math.Abs(player.velocity.Y) < 1;

        return isStill && isHooked;
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
