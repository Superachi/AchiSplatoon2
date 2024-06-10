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
    internal class SplatterBulletDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noLight = false;

            var random = Main.rand.NextFloat(-1, 1);
            dust.velocity.X += random;
            dust.velocity.Y += random;
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.scale -= 0.05f;
            dust.alpha -= (int)(dust.scale * 255);

            float light = 0.002f * dust.scale;
            Lighting.AddLight(dust.position, (dust.color.R * 0.5f * light), (dust.color.G * 0.5f * light), (dust.color.B * 0.5f * light));

            if (dust.scale < 0.1f)
            {
                dust.active = false;
            }
            return false;
        }
    }
}
