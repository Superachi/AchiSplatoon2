using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Dusts;

namespace AchiSplatoon2.Content.Projectiles.DualieProjectiles
{
    internal class VortexDualieShotProjectile : DualieShotProjectile
    {
        private Texture2D sprite;
        private Texture2D spriteOpaque;
        private Color bulletColor;

        public override void AfterSpawn()
        {
            base.AfterSpawn();
            dissolvable = false;

            colorOverride = new Color(60, 210, 250);
            bulletColor = (Color)colorOverride;
        }
        public override void AI()
        {
            if (Main.rand.NextBool(20))
            {
                var dustB = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: ModContent.DustType<ChargerBulletDust>(),
                    Velocity: Main.rand.NextVector2Circular(6, 6),
                    newColor: bulletColor,
                    Scale: Main.rand.NextFloat(0.8f, 1.6f));
                dustB.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Credits to: https://www.youtube.com/watch?v=cph82roy1_0
            if (sprite == null)
            {
                sprite = ModContent.Request<Texture2D>("AchiSplatoon2/Content/Assets/Textures/VortexDualieShot").Value;
            }

            if (spriteOpaque == null)
            {
                spriteOpaque = ModContent.Request<Texture2D>("AchiSplatoon2/Content/Assets/Textures/VortexDualieShotOpaque").Value;
            }

            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Vector2 origin = sprite.Size() / 2;
            Color color = bulletColor;
            Color blendColor = new Color((color.R * Color.White.R) / 255, (color.R + Color.White.G) / 255, (color.R + Color.White.B) / 255, 0);
            float scale = 2f;
            float rotation = Projectile.velocity.ToRotation();

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            Main.EntitySpriteDraw(sprite, position, null, color, rotation, origin, scale, SpriteEffects.None);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            Main.EntitySpriteDraw(spriteOpaque, position, null, blendColor, rotation, origin, scale, SpriteEffects.None);

            return base.PreDraw(ref lightColor);
        }
    }
}
