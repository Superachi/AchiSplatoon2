using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class BaseBombProjectile : BaseProjectile
    {
        protected override bool EnablePierceDamageFalloff { get => false; }
    }
}
