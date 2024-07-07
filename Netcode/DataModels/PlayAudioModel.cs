using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace AchiSplatoon2.Netcode.DataModels
{
    internal class PlayAudioModel
    {
        [JsonProperty]
        internal string soundPath;
        [JsonProperty]
        internal float volume;
        [JsonProperty]
        internal float pitchVariance;
        [JsonProperty]
        internal int maxInstances;
        [JsonProperty]
        internal float pitch;
        [JsonProperty]
        internal Vector2? position;

        public PlayAudioModel(string _soundPath, float _volume = 0.3f, float _pitchVariance = 0f, int _maxInstances = 1, float _pitch = 0f, Vector2? _position = null)
        {
            soundPath = _soundPath;
            volume = _volume;
            pitchVariance = _pitchVariance;
            maxInstances = _maxInstances;
            pitch = _pitch;
            position = _position;
        }
    }
}
