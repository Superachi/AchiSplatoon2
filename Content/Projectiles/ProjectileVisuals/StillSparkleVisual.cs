using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.ProjectileVisuals
{
    internal class StillSparkleVisual : BaseProjectile
    {
        private Texture2D _sparkleSprite;
        private Texture2D _glowSprite;
        private Texture2D _circleSprite;

        private float _rotateSpeed = 0;
        private float _alphaMod = 0;
        private float _drawScale = 0;
        private float _circleAlphaMod = 0;
        private float _circleDrawScale = 0;

        private bool _hideSparkle = false;

        private const int _stateCircleIn = 0;
        private const int _stateSlowdownSparkle = 1;
        private const int _stateFadeSparkle = 2;

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.friendly = false;
        }

        protected override void AfterSpawn()
        {
            dissolvable = false;
            UpdateCurrentColor(Owner.GetModPlayer<ColorChipPlayer>().GetColorFromInkPlayer());

            _sparkleSprite = TexturePaths.Medium4pSparkle.ToTexture2D();
            _glowSprite = TexturePaths.Glow100x.ToTexture2D();
            _glowSprite = TexturePaths.MediumCircle.ToTexture2D();

            Projectile.rotation = Main.rand.Next(0, 360);

            _rotateSpeed = 0f;
            _alphaMod = 1f;
            _drawScale = 1.6f;
            _circleAlphaMod = 2f;
            _circleDrawScale = 0f;
        }

        public void AdjustScale(float scale)
        {
            _drawScale *= scale;
            _circleDrawScale *= scale;
        }

        public void AdjustRotation(float rotation, float rotationSpeed = 0f)
        {
            Projectile.rotation = rotation;
            _rotateSpeed = rotationSpeed;
        }

        public void AdjustColor(Color color)
        {
            UpdateCurrentColor(color);
        }

        public void HideSparkle()
        {
            _hideSparkle = true;
        }

        public override void AI()
        {
            if (timeSpentAlive > 20)
            {
                Projectile.Kill();
                return;
            }

            Projectile.rotation += _rotateSpeed;
            _rotateSpeed *= 0.98f;

            _drawScale = MathHelper.Lerp(_drawScale, 0, 0.1f);
            _alphaMod = MathHelper.Lerp(_drawScale, 0, 0.1f);

            _circleDrawScale = MathHelper.Lerp(_circleDrawScale, 1, 0.1f);
            _circleAlphaMod = MathHelper.Lerp(_circleAlphaMod, 0, 0.2f);
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overWiresUI.Add(index);
        }

        public override void PostDraw(Color lightColor)
        {
            _sparkleSprite = TexturePaths.Medium4pSparkle.ToTexture2D();
            _glowSprite = TexturePaths.Glow100x.ToTexture2D();
            _circleSprite = TexturePaths.MediumCircle.ToTexture2D();

            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 4; i++)
            {
                Main.EntitySpriteDraw(
                    texture: _circleSprite,
                    position: Projectile.Center - Main.screenPosition,
                    sourceRectangle: null,
                    color: ColorHelper.ColorWithAlpha255(CurrentColor) * _circleAlphaMod,
                    rotation: 0,
                    origin: _circleSprite.Size() / 2,
                    scale: _circleDrawScale,
                    effects: SpriteEffects.None);
            }

            for (int i = 0; i < 3; i++)
            {
                if (_alphaMod > 0)
                {
                    if (!_hideSparkle)
                    {
                        var sparkleColor = ColorHelper.ColorWithAlpha255(i == 2 ? Color.White : CurrentColor) * _alphaMod * 0.7f;
                        if (i == 2) sparkleColor = ColorHelper.ColorWithAlpha255(Color.White) * _alphaMod;

                        Main.EntitySpriteDraw(
                            texture: _sparkleSprite,
                            position: Projectile.Center - Main.screenPosition,
                            sourceRectangle: null,
                            color: sparkleColor,
                            rotation: Projectile.rotation,
                            origin: _sparkleSprite.Size() / 2,
                            scale: _drawScale - (i * 0.2f),
                            effects: SpriteEffects.None);
                    }

                    Main.EntitySpriteDraw(
                        texture: _glowSprite,
                        position: Projectile.Center - Main.screenPosition,
                        sourceRectangle: null,
                        color: ColorHelper.ColorWithAlpha255(CurrentColor) * _alphaMod * 0.4f,
                        rotation: 0,
                        origin: _glowSprite.Size() / 2,
                        scale: _drawScale * (i * 0.7f),
                        effects: SpriteEffects.None);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
