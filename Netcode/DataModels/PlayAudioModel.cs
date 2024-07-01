using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Netcode.DataModels
{
    internal class PlayAudioModel
    {
        internal string soundPath;
        internal float volume;
        internal float pitchVariance;
        internal int maxInstances;
        internal float pitch;
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
