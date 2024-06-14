using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class GrizzcoStringerCharge : TriStringerCharge
    {
        protected override float[] ChargeTimeThresholds { get => [40f, 80f]; }
        protected override float ShotgunArc { get => 12f; }
        protected override int ProjectileCount { get => 9; }
    }
}
