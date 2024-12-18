using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.ModConfigs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class HudProjectile : BaseProjectile
    {
        private Texture2D? barBack;
        private Texture2D? barFront;

        private float _visualInkQuotient = 1f;
        private float _InkTankAlpha = 1f;
        private float _InkTankAlphaGoal = 1f;
        private Player _owner;

        private string _overheadText = "";
        private float _overheadTextScale = 0f;
        private int _overheadTextDisplayTime = 0;
        private Color _overheadTextColor = Color.White;

        InkTankPlayer InkTankMp => _owner.GetModPlayer<InkTankPlayer>();
        SquidPlayer SquidMp => _owner.GetModPlayer<SquidPlayer>();

        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.tileCollide = false;

            Projectile.timeLeft = 2;
        }

        protected override void AfterSpawn()
        {
            dissolvable = false;
            _owner = GetOwner();
        }

        public override void AI()
        {
            Projectile.timeLeft = 2;
            Projectile.Center = _owner.Center;

            LerpInkTankAlpha();
            ScaleText();
        }

        private void LerpInkTankAlpha()
        {
            _InkTankAlphaGoal = 0f;
            if (SquidMp.IsSquid())
            {
                _InkTankAlphaGoal = 1f;
            }
            else if (!InkTankMp.HasMaxInk())
            {
                _InkTankAlphaGoal = 0.4f;
            }

            _InkTankAlpha = MathHelper.Lerp(_InkTankAlpha, _InkTankAlphaGoal, 0.2f);
        }

        private void ScaleText()
        {
            if (_overheadTextDisplayTime > 0)
            {
                _overheadTextDisplayTime--;
                if (_overheadTextScale < 1)
                {
                    _overheadTextScale = MathHelper.Lerp(_overheadTextScale, 1, 0.2f);
                }
            }

            if (_overheadTextDisplayTime <= 0)
            {
                if (_overheadTextScale > 0)
                {
                    _overheadTextScale = MathHelper.Lerp(_overheadTextScale, 0, 0.2f);
                }
            }
        }

        /// <summary>
        /// Sets the text that's briefly displayed above the player. It's preferrable to set this via the <see cref="HudPlayer"/> instead.
        /// </summary>
        public void SetOverheadText(string text, int displayTime, Color? color = null)
        {
            _overheadText = text;
            _overheadTextDisplayTime = displayTime;
            _overheadTextScale = 0;
            _overheadTextColor = color ?? Color.White;
        }

        public bool IsTextActive()
        {
            return _overheadTextDisplayTime > 0;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!IsThisClientTheProjectileOwner()) return false;
            if (_owner.dead) return false;

            // Prepare to draw
            barBack = ModContent.Request<Texture2D>("AchiSplatoon2/Content/UI/InkTank/InkTankBack").Value;
            barFront = ModContent.Request<Texture2D>("AchiSplatoon2/Content/UI/InkTank/InkTankFront").Value;

            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 position = _owner.Center.Round() - Main.screenPosition + new Vector2(-70 * _owner.direction, 0 + _owner.gfxOffY);
            Vector2 origin = barBack.Size() / 2;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            // Draw bar background
            Main.EntitySpriteDraw(barBack, new Vector2((int)position.X, (int)position.Y), null, Color.White * _InkTankAlpha, 0, origin, 1f, SpriteEffects.None);

            // Draw bar foreground
            float lerpAmount = 0.2f;
            if (SquidMp.IsFlat() && !InkTankMp.HasMaxInk())
            {
                lerpAmount = (float)Math.Sin(Main.time / 4) * 0.2f + 0.2f;
            }
            Color chipColor = ColorHelper.ColorWithAlpha255(_owner.GetModPlayer<ColorChipPlayer>().GetColorFromChips());
            Color finalColor = ColorHelper.LerpBetweenColorsPerfect(chipColor, Color.White, lerpAmount);

            var realInkQuotient = Math.Min(InkTankMp.InkQuotient(), 1);
            _visualInkQuotient = MathHelper.Lerp(_visualInkQuotient, realInkQuotient, 0.1f);
            var verticalSize = (int)(44 * _visualInkQuotient);

            spriteBatch.Draw(
                barFront,
                new Rectangle(
                    (int)position.X - (int)origin.X + 6,
                    (int)position.Y + 23 - verticalSize,
                    (int)barFront.Size().X,
                    (int)verticalSize),
                ColorHelper.LerpBetweenColorsPerfect(finalColor, Color.White, 0.2f) * _InkTankAlpha);

            if (!ModContent.GetInstance <ClientConfig>().HideInkTankPercentage)
            {
                Utils.DrawBorderString(
                    Main.spriteBatch, $"{(GetOwnerModPlayer<InkTankPlayer>().InkAmount).ToString("0")}%", position + new Vector2(0, 40),
                    Color.White * _InkTankAlpha,
                    anchorx: 0.5f);
            }

            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            if (!IsThisClientTheProjectileOwner()) return;
            if (_owner.dead) return;

            Utils.DrawBorderString(
                Main.spriteBatch,
                _overheadText,
                GetOwner().Center - Main.screenPosition + new Vector2(0, -50 + GetOwner().gfxOffY),
                _overheadTextColor,
                scale: _overheadTextScale,
                anchorx: 0.5f,
                anchory: 0.5f);
        }
    }
}
