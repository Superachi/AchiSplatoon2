using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.GlobalProjectiles
{
    internal class BaseGlobalProjectile : GlobalProjectile
    {
        public bool deflected = false;
        public override bool InstancePerEntity => true;
    }
}
