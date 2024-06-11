using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class TriStringerProjectileWeak : TriStringerProjectile
    {
        protected override bool CanStick { get => false; }
    }
}
