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
        private Texture2D _circleSprite;

        private float _rotateSpeed = 0;
        private float _alphaMod = 0;
        private float _drawScale = 0;
        private float _circleAlphaMod = 0;
        private float _circleDrawScale = 0;
        private float _circleShrinkSpeed = 0;

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
            if (Owner.dead)
            {
                Projectile.Kill();
                return;
            }

            UpdateCurrentColor(Owner.GetModPlayer<ColorChipPlayer>().GetColorFromInkPlayer());

            _sparkleSprite = TexturePaths.Medium4pSparkle.ToTexture2D();
            _glowSprite = TexturePaths.Glow100x.ToTexture2D();
            _glowSprite = TexturePaths.MediumCircle.ToTexture2D();

            Projectile.rotation = Main.rand.Next(0, 360);

            _rotateSpeed = 0.2f;
            _alphaMod = 0f;
            _drawScale = 0f;
            _circleAlphaMod = 0f;
            _circleDrawScale = 3f;
            _circleShrinkSpeed = 0.05f;
        }

        public override void AI()
        {
            UpdateCurrentColor(Owner.GetModPlayer<ColorChipPlayer>().GetColorFromInkPlayer());

            if (Owner.GetModPlayer<SquidPlayer>().IsSquid())
            {
                Projectile.Center = Owner.Center + new Vector2(0, 16 + Owner.gfxOffY);
            }
            else
            {
                Projectile.Center = Owner.Center + new Vector2(0, -16 + Owner.gfxOffY);
            }

            Projectile.rotation += _rotateSpeed;
            _rotateSpeed *= 0.98f;

            // Dust
            if (state != _stateCircleIn)
            {
                if (Main.rand.NextBool(1 + timeSpentAlive / 4))
                {
                    DustHelper.NewDust(
                        position: Owner.Center + Main.rand.NextVector2Circular(60 * _drawScale, 60 * _drawScale),
                        dustType: DustID.YellowStarDust,
                        velocity: new Vector2(0, Main.rand.NextFloat(-2, -5)),
                        color: Color.White,
                        scale: Main.rand.NextFloat(1f, 2f));
                }
            }

            switch (state)
            {
                case _stateCircleIn:
                    _circleAlphaMod = MathHelper.Lerp(_circleAlphaMod, 1f, 0.2f);

                    // Fade circle out
                    if (timeSpentInState > 20)
                    {
                        _circleAlphaMod = MathHelper.Lerp(_circleAlphaMod, 0f, 0.1f);
                    }

                    // Shrink circle
                    _circleDrawScale -= _circleShrinkSpeed;
                    _circleShrinkSpeed += 0.01f;
                    if (_circleDrawScale < 0) _circleDrawScale = 0;

                    if (timeSpentInState > 15 && _alphaMod < 1)
                    {
                        _alphaMod += 0.3f;
                    }

                    if (timeSpentInState == 14)
                    {
                        SoundHelper.PlayAudio(SoundID.DD2_JavelinThrowersAttack, 0.6f, pitch: 0.2f, maxInstances: 1, position: Owner.Center);
                    }

                    if (timeSpentInState == 20)
                    {
                        BurstEffect();
                    }

                    // Grow sparkle
                    if (timeSpentInState > 20)
                    {
                        _drawScale = MathHelper.Lerp(_drawScale, 1f, 0.5f);
                    }

                    if (timeSpentInState > 60)
                    {
                        _circleAlphaMod = 0;
                        SetState(_stateSlowdownSparkle);
                        return;
                    }

                    break;

                case _stateSlowdownSparkle:
                    // Shrink sparkle
                    _drawScale = MathHelper.Lerp(_drawScale, 1f, 0.05f);

                    if (timeSpentInState > 30)
                    {
                        SetState(_stateFadeSparkle);
                    }

                    break;

                case _stateFadeSparkle:
                    // Fade/shrink sparkle
                    _alphaMod -= 0.1f;
                    if (_alphaMod < 0) _alphaMod = 0;

                    _drawScale -= 0.1f;
                    if (_drawScale < 0) _drawScale = 0;

                    if (timeSpentAlive > 180)
                    {
                        Projectile.Kill();
                        return;
                    }

                    break;
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

                    Main.EntitySpriteDraw(
                        texture: _glowSprite,
                        position: Projectile.Center - Main.screenPosition,
                        sourceRectangle: null,
                        color: ColorHelper.ColorWithAlpha255(CurrentColor) * 0.6f,
                        rotation: 0,
                        origin: _glowSprite.Size() / 2,
                        scale: _drawScale * (i * 0.5f),
                        effects: SpriteEffects.None);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }

        private void BurstEffect()
        {
            Owner.GetModPlayer<HudPlayer>()!.SetOverheadText("Special charged!", 90, color: new Color(255, 155, 0));

            // SFX
            SoundHelper.PlayAudio(SoundPaths.SpecialReady.ToSoundStyle(), 0.5f, maxInstances: 1, position: Owner.Center);

            SoundHelper.PlayAudio(SoundID.Item60, 0.7f, maxInstances: 1, position: Owner.Center);
            SoundHelper.PlayAudio(SoundID.Item100, 0.3f, pitchVariance: 0.2f, maxInstances: 1, position: Owner.Center);
            SoundHelper.PlayAudio(SoundID.Item117, 0.3f, pitchVariance: 0.2f, maxInstances: 1, position: Owner.Center);

            // Burst dust
            var loopCount = 20;
            for (int i = 0; i < loopCount; i++)
            {
                var velocity = new Vector2(Main.rand.Next(10, 16), 0);
                velocity = WoomyMathHelper.AddRotationToVector2(velocity, i * (360f / loopCount) + Main.rand.Next(-15, 15));

                var color = CurrentColor.IncreaseHueBy(Main.rand.NextFloat(-10, 10));

                var d = DustHelper.NewDust(
                    position: Projectile.Center,
                    dustType: DustID.FireworksRGB,
                    velocity: velocity,
                    color: color,
                    scale: Main.rand.NextFloat(0.6f, 1.2f));
                d.noGravity = true;
            }
        }
    }
}
