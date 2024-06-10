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
    internal class SplatterDropletDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noLight = false;
            dust.velocity.X += Main.rand.NextFloat(-1, 1);
            dust.velocity.Y += Main.rand.NextFloat(-1, 3);
        }

        public override bool Update(Dust dust)
        {
            dust.velocity.Y += 0.1f;
            dust.position += dust.velocity;
            dust.scale -= 0.05f;
            dust.alpha -= (int)(dust.scale * 255);
            if (dust.scale < 0.1f)
            {
                dust.active = false;
            }
            return false;
        }
    }
}
