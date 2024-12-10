using AchiSplatoon2.Content.Dusts.CustomDataObjects;
using Terraria;

namespace AchiSplatoon2.Content.Dusts
{
    internal class SplatterBulletDust : BaseDust
    {
        public override void AfterSpawn(Dust dust)
        {
            defaultScaleSpeed = -1f;
            // dust.velocity += Main.rand.NextVector2Circular(1, 1);
        }

        public override void CustomUpdate(Dust dust)
        {
            var customData = dust.customData;
            
            if (customData is BaseDustData data)
            {
                dust.scale += data.scaleIncrement;

                if (data.emitLight)
                {
                    float light = 0.001f * dust.scale;
                    Lighting.AddLight(dust.position, (dust.color.R * 0.5f * light), (dust.color.G * 0.5f * light), (dust.color.B * 0.5f * light));
                }

                if (data.gravity != 0)
                {
                    dust.velocity.Y += data.gravity;
                }

                dust.velocity *= data.frictionMult;
            }
            else
            {
                dust.scale += defaultScaleSpeed;
            }

            dust.alpha -= (int)(dust.scale * 255);

            if (dust.scale < 0.01f)
            {
                dust.active = false;
            }

            return;
        }
    }
}
