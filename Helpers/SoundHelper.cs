using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria;
using Microsoft.Xna.Framework;
using AchiSplatoon2.Netcode.DataModels;
using System.Security.Cryptography.X509Certificates;

namespace AchiSplatoon2.Helpers
{
    internal static class SoundHelper
    {
        private static void PlaySoundFinal(SoundStyle soundStyle, float volume = 0.3f, float pitchVariance = 0f, int maxInstances = 1, float pitch = 0f, Vector2? position = null)
        {
            if (position == null)
            {
                Player player = Main.LocalPlayer;
                position = player.Center;
            }

            var sound = soundStyle with
            {
                Volume = volume,
                PitchVariance = pitchVariance,
                MaxInstances = maxInstances,
                Pitch = pitch,
            };

            SoundEngine.PlaySound(sound, position);
        }

        public static void PlayAudio(string soundPath, float volume = 0.3f, float pitchVariance = 0f, int maxInstances = 1, float pitch = 0f, Vector2? position = null)
        {
            var style = new SoundStyle($"AchiSplatoon2/Content/Assets/Sounds/{soundPath}");
            PlaySoundFinal(style, volume, pitchVariance, maxInstances, pitch, position);
        }

        public static void PlayAudio(SoundStyle soundStyle, float volume = 0.3f, float pitchVariance = 0f, int maxInstances = 1, float pitch = 0f, Vector2? position = null)
        {
            PlaySoundFinal(soundStyle, volume, pitchVariance, maxInstances, pitch, position);
        }

        public static void PlayAudio(PlayAudioModel m)
        {
            PlayAudio(soundPath: m.soundPath, volume: m.volume, pitchVariance: m.pitchVariance, maxInstances: m.maxInstances, pitch: m.pitch, position: m.position);
        }
    }
}
