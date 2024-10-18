using Terraria;

namespace AchiSplatoon2.Content.Dusts
{
    internal class SplatterDropletDust : BaseDust
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
