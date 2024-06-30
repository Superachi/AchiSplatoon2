using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchiSplatoon2.Netcode.DataModels
{
    internal class ExplosionDustModel
    {
        [JsonProperty]
        internal readonly float dustMaxVelocity;
        [JsonProperty]
        internal readonly int dustAmount;
        [JsonProperty]
        internal readonly float minScale;
        [JsonProperty]
        internal readonly float maxScale;
        [JsonProperty]
        internal readonly int radiusModifier;

        internal ExplosionDustModel(float _dustMaxVelocity, int _dustAmount, float _minScale, float _maxScale, int _radiusModifier)
        {
            dustMaxVelocity = _dustMaxVelocity;
            dustAmount = _dustAmount;
            minScale = _minScale;
            maxScale = _maxScale;
            radiusModifier = _radiusModifier;
        }
    }
}
