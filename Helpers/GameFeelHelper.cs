using System;
using Terraria;
using Terraria.Graphics.CameraModifiers;

namespace AchiSplatoon2.Helpers
{
    internal class GameFeelHelper
    {
        public static void ShakeScreenNearPlayer(Player player, bool localOnly = true)
        {
            if (localOnly && !NetHelper.IsPlayerSameAsLocalPlayer(player)) return;

            PunchCameraModifier modifier = new PunchCameraModifier(
                startPosition: player.Center,
                direction: (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(),
                strength: 3f,
                vibrationCyclesPerSecond: 8f,
                frames: 10,
                distanceFalloff: 80f);
            Main.instance.CameraModifiers.Add(modifier);
        }
    }
}
