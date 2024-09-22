using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace AchiSplatoon2.Content.Dusts
{
    internal class SplatterBulletLastingDust : BaseDust
    {
        public virtual float ShrinkRate { get; set; } = 0.08f;
        public float Friction { get; set; } = 0.9f;

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.velocity *= Friction;

            if (Math.Abs(dust.velocity.X) <= 0.2 && Math.Abs(dust.velocity.Y) <= 0.2)
            {
                dust.scale -= ShrinkRate;
            }

            float light = 0.002f * dust.scale;
            Lighting.AddLight(dust.position, (dust.color.R * 0.5f * light), (dust.color.G * 0.5f * light), (dust.color.B * 0.5f * light));

            if (dust.scale < 0.05f) dust.active = false;
            return false;
        }

        public override bool PreDraw(Dust dust)
        {
            DrawDust(dust.dustIndex, dust.color, rotation: 0f, considerWorldLight: false, blendState: BlendState.Additive);
            return false;
        }
    }
}
