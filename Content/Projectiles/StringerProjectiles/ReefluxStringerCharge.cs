using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class ReefluxStringerCharge : TriStringerCharge
    {
        protected override float[] ChargeTimeThresholds { get => [20f, 40f]; }
        protected override float ShotgunArc { get => 5f; }
        protected override int ProjectileCount { get => 3; }
        protected override bool AllowStickyProjectiles { get => false; }
    }
}
