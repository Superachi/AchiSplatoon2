using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;

namespace AchiSplatoon2.Helpers
{
    internal static class SoundHelper
    {
        private static SlotId PlaySoundFinal(SoundStyle soundStyle, float volume = 0.3f, float pitchVariance = 0f, int maxInstances = 1, float pitch = 0f, Vector2? position = null)
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

            return SoundEngine.PlaySound(sound, position);
        }

        private static SlotId PlayAudio(string soundPath, float volume = 0.3f, float pitchVariance = 0f, int maxInstances = 1, float pitch = 0f, Vector2? position = null)
        {
            var style = new SoundStyle(soundPath);
            return PlaySoundFinal(style, volume, pitchVariance, maxInstances, pitch, position);
        }

        public static SlotId PlayAudio(SoundStyle soundStyle, float volume = 0.3f, float pitchVariance = 0f, int maxInstances = 1, float pitch = 0f, Vector2? position = null)
        {
            return PlaySoundFinal(soundStyle, volume, pitchVariance, maxInstances, pitch, position);
        }

        public static SlotId PlayAudio(PlayAudioModel m)
        {
            return PlayAudio(soundPath: m.soundPath, volume: m.volume, pitchVariance: m.pitchVariance, maxInstances: m.maxInstances, pitch: m.pitch, position: m.position);
        }

        public static bool TryGetActiveSound(SlotId? slotId, out ActiveSound? activeSound)
        {
            activeSound = null;
            if (slotId == null) return false;

            return SoundEngine.TryGetActiveSound((SlotId)slotId!, out activeSound);
        }

        public static void StopSoundIfActive(SlotId? slotId)
        {
            var soundExists = TryGetActiveSound(slotId, out var sound);
            if (soundExists)
            {
                sound?.Stop();
            }
        }
    }
}
