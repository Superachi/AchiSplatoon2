using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.ProjectileVisuals
{
    internal class SpecialSparkleVisual : BaseProjectile
    {
        private Texture2D _sparkleSprite;
        private Texture2D _glowSprite;

        private float _rotateSpeed = 0;
        private float _drawScale = 0;
        private float _alphaMod = 0;

        protected override void AfterSpawn()
        {
            UpdateCurrentColor(Owner.GetModPlayer<ColorChipPlayer>().GetColorFromChips());

            _sparkleSprite = TexturePaths.Medium4pSparkle.ToTexture2D();
            _glowSprite = TexturePaths.Glow100x.ToTexture2D();

            Projectile.rotation = Main.rand.Next(0, 360);

            _rotateSpeed = 0.2f;
            _drawScale = 2f;
            _alphaMod = 0f;

            // SFX
            SoundHelper.PlayAudio(SoundPaths.SpecialReady.ToSoundStyle(), 0.5f, maxInstances: 1, position: Owner.Center);

            SoundHelper.PlayAudio(SoundID.Item60, 0.7f, maxInstances: 1, position: Owner.Center);
            SoundHelper.PlayAudio(SoundID.Item100, 0.3f, pitchVariance: 0.2f, maxInstances: 1, position: Owner.Center);
            SoundHelper.PlayAudio(SoundID.Item117, 0.3f, pitchVariance: 0.2f, maxInstances: 1, position: Owner.Center);

            // Burst dust
            var loopCount = 20;
            for (int i = 0; i < loopCount; i++)
            {
                var velocity = new Vector2(Main.rand.Next(8, 12), 0);
                velocity = WoomyMathHelper.AddRotationToVector2(velocity, i * (360f / loopCount) + Main.rand.Next(-20, 20));

                DustHelper.NewDust(
                    position: Projectile.Center,
                    dustType: DustID.FireworksRGB,
                    velocity: velocity,
                    color: CurrentColor,
                    scale: Main.rand.NextFloat(0.6f, 1.2f),
                    data: new(gravity: -1));
            }
        }

        public override void AI()
        {
            UpdateCurrentColor(Owner.GetModPlayer<ColorChipPlayer>().GetColorFromChips());

            if (Owner.GetModPlayer<SquidPlayer>().IsSquid())
            {
                Projectile.Center = Owner.Center + new Vector2(0, 16 + Owner.gfxOffY);
            }
            else
            {
                Projectile.Center = Owner.Center + new Vector2(0, -16 + Owner.gfxOffY);
            }

            // Sprite animation
            Projectile.rotation += _rotateSpeed;
            _rotateSpeed *= 0.97f;
            _drawScale = MathHelper.Lerp(_drawScale, 1f, 0.05f);

            if (timeSpentAlive < 10 && _alphaMod < 1f)
            {
                _alphaMod += 0.1f;
            }

            if (timeSpentAlive > 60)
            {
                _alphaMod -= 0.02f;
                _drawScale -= 0.05f;
            }

            if (timeSpentAlive > 180)
            {
                Projectile.Kill();
                return;
            }

            // Dust
            if (Main.rand.NextBool(1 + timeSpentAlive / 4))
            {
                DustHelper.NewDust(
                    position: Owner.Center + Main.rand.NextVector2Circular(60 * _drawScale, 60 *_drawScale),
                    dustType: DustID.YellowStarDust,
                    velocity: new Vector2(0, Main.rand.NextFloat(-2, -5)),
                    color: Color.White,
                    scale: Main.rand.NextFloat(1f, 2f));
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public override void PostDraw(Color lightColor)
        {
            _sparkleSprite = TexturePaths.Medium4pSparkle.ToTexture2D();
            _glowSprite = TexturePaths.Glow100x.ToTexture2D();

            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            var lerpColor = ColorHelper.LerpBetweenColorsPerfect(CurrentColor, Color.White, 0f);

            for (int i = 0; i < 3; i++)
            {
                Main.EntitySpriteDraw(
                    texture: _sparkleSprite,
                    position: Projectile.Center - Main.screenPosition,
                    sourceRectangle: null,
                    color: ColorHelper.ColorWithAlpha255(lerpColor) * _alphaMod * 0.7f,
                    rotation: Projectile.rotation,
                    origin: _sparkleSprite.Size() / 2,
                    scale: _drawScale - (i * 0.2f),
                    effects: SpriteEffects.None);

                Main.EntitySpriteDraw(
                    texture: _glowSprite,
                    position: Projectile.Center - Main.screenPosition,
                    sourceRectangle: null,
                    color: ColorHelper.ColorWithAlpha255(lerpColor) * 0.6f,
                    rotation: Projectile.rotation,
                    origin: _glowSprite.Size() / 2,
                    scale: _drawScale * (i * 0.5f),
                    effects: SpriteEffects.None);
            }

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
