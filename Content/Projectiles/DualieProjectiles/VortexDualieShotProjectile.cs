using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.DualieProjectiles
{
    internal class VortexDualieShotProjectile : DualieShotProjectile
    {
        private Texture2D sprite;
        private Color bulletColor;
        private DualiePlayer dualieMP;
        private int dustId;

        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            dissolvable = false;

            dualieMP = GetOwnerModPlayer<DualiePlayer>();
            if (dualieMP.isTurret)
            {
                colorOverride = new Color(140, 70, 255).IncreaseHueBy(Main.rand.Next(-40, 40));
                dustId = 181;
            }
            else
            {
                dustId = 111;
                colorOverride = new Color(0, 240, 170).IncreaseHueBy(Main.rand.Next(-20, 20));
            }

            bulletColor = (Color)colorOverride;
        }
        public override void AI()
        {
            if (timeSpentAlive % 20 == 0 && Main.rand.NextBool(5))
            {
                Color dustColor = GenerateInkColor();
                Dust dust = Dust.NewDustDirect(Position: Projectile.Center, Type: dustId, Width: 1, Height: 1, newColor: Color.White, Scale: Main.rand.NextFloat(0.8f, 1.2f));
                dust.noGravity = !Main.rand.NextBool(5);
                dust.velocity = Projectile.velocity * 2 + Main.rand.NextVector2Circular(2f, 2f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        protected override void CreateDustOnSpawn()
        {
        }

        protected override void CreateDustOnDespawn()
        {
            for (int i = 0; i < 5; i++)
            {
                Color dustColor = GenerateInkColor();
                Dust dust = Dust.NewDustDirect(Position: Projectile.position, Type: dustId, Width: 1, Height: 1, newColor: Color.White, Scale: Main.rand.NextFloat(1.2f, 1.6f));
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(5f, 5f);
            }
        }

        protected override void PlayShootSound()
        {
            var dualieMP = GetOwner().GetModPlayer<DualiePlayer>();
            if (dualieMP.isTurret)
            {
                PlayAudio(SoundID.Item91, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 5, pitch: 1);
                PlayAudio(SoundID.Item115, volume: 0.1f, pitchVariance: 0f, maxInstances: 5, pitch: 1f);
                PlayAudio(SoundID.Item39, volume: 0.1f, pitchVariance: 0.2f, maxInstances: 10, pitch: 0f);
            }
            else
            {
                PlayAudio(SoundID.Item75, volume: 0.1f, pitchVariance: 0.5f, maxInstances: 5, pitch: 0f);
                PlayAudio(SoundID.Item91, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 5, pitch: 0.5f);
                PlayAudio(SoundID.Item39, volume: 0.3f, pitchVariance: 0.2f, maxInstances: 10, pitch: -0.5f);
                PlayAudio(SoundID.Item60, volume: 0.1f, pitchVariance: 0.2f, maxInstances: 10, pitch: 0.8f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Credits to: https://www.youtube.com/watch?v=cph82roy1_0
            if (sprite == null)
            {
                sprite = TexturePaths.VortexDualieShot.ToTexture2D();
            }

            var alpha = MathHelper.Min(timeSpentAlive, 60) / 60f;
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Vector2 origin = sprite.Size() / 2;
            Color color = bulletColor;
            Color blendColor = new Color((color.R * Color.White.R) / 255, (color.R + Color.White.G) / 255, (color.R + Color.White.B) / 255, 0);
            float scale = 2f;
            float rotation = Projectile.velocity.ToRotation();

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            Main.EntitySpriteDraw(sprite, position, null, color * alpha, rotation, origin, scale, SpriteEffects.None);

            return base.PreDraw(ref lightColor);
        }
    }
}
