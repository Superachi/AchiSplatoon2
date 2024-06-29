using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchiSplatoon2.Netcode.DataModels
{
    internal class ExplosionDustModel
    {
        internal readonly float dustMaxVelocity;
        internal readonly int dustAmount;
        internal readonly float minScale;
        internal readonly float maxScale;
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
