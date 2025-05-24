using AchiSplatoon2.ModConfigs;
using System;
using Terraria;
using Terraria.Graphics.CameraModifiers;
using Terraria.ModLoader;

namespace AchiSplatoon2.Helpers
{
    internal class GameFeelHelper
    {
        public static void ShakeScreenNearPlayer(Player player, bool localOnly = true, float strength = 3f, float speed = 8f, int duration = 10)
        {
            if (ModContent.GetInstance<ClientConfig>().DisableScreenshake) return;

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
