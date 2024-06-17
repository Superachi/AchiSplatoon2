using Terraria;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace AchiSplatoon2.Content.Dusts
{
    internal class BlasterExplosionDust : ModDust
    {
        public virtual Vector2 RotationSpeedRange { get; set; } = new Vector2(-0.05f, 0.05f);
        private float RotationSpeed = 0f;
        public virtual float ScaleSpeed { get; set; } = -0.05f;
        public virtual float Friction { get; set; } = 0.2f;
        public override void OnSpawn(Dust dust)
        {
            RotationSpeed = Main.rand.NextFloat(RotationSpeedRange.X, RotationSpeedRange.Y);
            dust.noLight = false;

            var random = Main.rand.NextFloat(-1, 1);
            dust.velocity.X += random;
            dust.velocity.Y += random;
        }

        public override bool Update(Dust dust)
        {
            if (Friction > 0)
            {
                dust.velocity = Vector2.Lerp(dust.velocity, Vector2.Zero, Friction);
            }
            dust.rotation += RotationSpeed;
            dust.position += dust.velocity;
            dust.scale += ScaleSpeed;
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
