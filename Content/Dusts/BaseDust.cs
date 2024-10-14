using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Dusts
{
    internal class BaseDust : ModDust
    {
        protected void DrawDust(int dustIndex, Color inkColor, float rotation, float scale = 1f, float alphaMod = 1, bool considerWorldLight = true, BlendState blendState = null)
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

            var spriteBatch = Main.spriteBatch;
            if (blendState != null)
            {
                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                spriteBatch.Draw(Texture2D.Value, position, dust.frame, finalColor * 0.5f, dust.rotation, new Vector2(4f, 4f), dust.scale, SpriteEffects.None, 0f);

                spriteBatch.End();
                spriteBatch.Begin(default, blendState, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                spriteBatch.Draw(Texture2D.Value, position, dust.frame, finalColor * 0.6f, dust.rotation, new Vector2(4f, 4f), dust.scale, SpriteEffects.None, 0f);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            } else
            {
                spriteBatch.Draw(Texture2D.Value, position, dust.frame, finalColor, dust.rotation, new Vector2(4f, 4f), dust.scale, SpriteEffects.None, 0f);
            }
        }

        public override bool PreDraw(Dust dust)
        {
            DrawDust(dust.dustIndex, dust.color, rotation: 0f, considerWorldLight: false, blendState: BlendState.Additive);
            return false;
        }
    }
}
