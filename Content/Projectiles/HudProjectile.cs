using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class HudProjectile : BaseProjectile
    {
        private Texture2D? barBack;
        private Texture2D? barFront;

        private float _visualInkQuotient = 1f;
        private float _InkTankAlpha = 1f;
        private Player _owner;

        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.tileCollide = false;

            Projectile.timeLeft = 2;
        }

        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            _owner = GetOwner();
        }

        public override void AI()
        {
            Projectile.timeLeft = 2;
            Projectile.Center = _owner.Center;

            SquidPlayer squidPlayer = _owner.GetModPlayer<SquidPlayer>();
            _InkTankAlpha = squidPlayer.IsSquid() ? 1f : 0.2f;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            if (!IsThisClientTheProjectileOwner()) return;

            SquidPlayer squidPlayer = _owner.GetModPlayer<SquidPlayer>();
            InkTankPlayer inkTankPlayer = _owner.GetModPlayer<InkTankPlayer>();
            var realInkQuotient = Math.Min(inkTankPlayer.InkQuotient(), 1);
            if (!squidPlayer.IsSquid() && realInkQuotient == 1) return;

            barBack = ModContent.Request<Texture2D>("AchiSplatoon2/Content/UI/InkTank/InkTankBack").Value;
            barFront = ModContent.Request<Texture2D>("AchiSplatoon2/Content/UI/InkTank/InkTankFront").Value;

            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 position = WoomyMathHelper.RoundVector2(_owner.Center) - Main.screenPosition + new Vector2(-70 * _owner.direction, 0 + _owner.gfxOffY);
            Vector2 origin = barBack.Size() / 2;

            Color color = ColorHelper.ColorWithAlpha255(_owner.GetModPlayer<ColorChipPlayer>().GetColorFromChips());

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(barBack, new Vector2((int)position.X, (int)position.Y), null, Color.White * _InkTankAlpha, 0, origin, 1f, SpriteEffects.None);

            _visualInkQuotient = MathHelper.Lerp(_visualInkQuotient, realInkQuotient, 0.1f);
            var verticalSize = (int)(44 * _visualInkQuotient);

            spriteBatch.Draw(
                barFront,
                new Rectangle(
                    (int)position.X - (int)origin.X + 6,
                    (int)position.Y + 23 - verticalSize,
                    (int)barFront.Size().X,
                    (int)verticalSize),
                ColorHelper.LerpBetweenColorsPerfect(color * _InkTankAlpha, Color.White * 0.75f, 0.2f));

            //Utils.DrawBorderString(
            //     Main.spriteBatch, $"{(GetOwnerModPlayer<InkTankPlayer>().InkAmount).ToString("0.0")}%", position + new Vector2(0, 40),
            //     ColorHelper.ColorWithAlpha(Color.Gray, 6),
            //     anchorx: 0.5f);
        }
    }
}
