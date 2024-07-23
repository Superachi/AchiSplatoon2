using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Dusts
{
    internal class SplatterBulletLastingDust : ModDust
    {
        public virtual float ShrinkRate { get; set; } = 0.05f;
        public float Friction { get; set; } = 0.9f;

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.velocity *= Friction;

            if (dust.velocity.X <= 0.2 && dust.velocity.Y <= 0.2)
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
            DrawDust(dust.dustIndex, dust.color, rotation: 0f, considerWorldLight: false);
            return false;
        }

        protected void DrawDust(int dustIndex, Color inkColor, float rotation, float scale = 1f, float alphaMod = 1, bool considerWorldLight = true)
        {
            Dust dust = Main.dust[dustIndex];

            Vector2 position = dust.position - Main.screenPosition;

            // The light value in the world
            var lightInWorld = Color.White;
            if (considerWorldLight)
            {
                lightInWorld = Lighting.GetColor(dust.position.ToTileCoordinates());
            }
            var finalColor = new Color(inkColor.R * lightInWorld.R / 255, inkColor.G * lightInWorld.G / 255, inkColor.B * lightInWorld.G / 255);

            Main.spriteBatch.Draw(Texture2D.Value, position, dust.frame, finalColor, dust.rotation, new Vector2(4f, 4f), dust.scale, SpriteEffects.None, 0f);
        }
    }
}
