using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Dusts
{
    internal class BlasterTrailDust : SplatterBulletDust
    {
        public override float ScaleSpeed { get; set; } = -0.2f;
    }
}
