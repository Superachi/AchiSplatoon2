using System;
using Terraria;
using Terraria.Graphics.CameraModifiers;

namespace AchiSplatoon2.Helpers
{
    internal class GameFeelHelper
    {
        public static void ShakeScreenNearPlayer(Player player, bool localOnly = true, float strength = 3f, float speed = 8f, int duration = 10)
        {
            if (localOnly && !NetHelper.IsPlayerSameAsLocalPlayer(player)) return;

            PunchCameraModifier modifier = new PunchCameraModifier(
                startPosition: player.Center,
                direction: (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(),
                strength: strength,
                vibrationCyclesPerSecond: speed,
                frames: duration,
                distanceFalloff: 80f);
            Main.instance.CameraModifiers.Add(modifier);
        }
    }
}
