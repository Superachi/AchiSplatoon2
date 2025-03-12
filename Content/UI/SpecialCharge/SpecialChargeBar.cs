using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace AchiSplatoon2.Content.UI.SpecialCharge
{
    internal class SpecialChargeBar : UIState
    {
        private UIElement area;
        private UIImage barFrame;
        private readonly UIImage barFill;
        private Color barColor;

        public override void OnInitialize()
        {
            // Explanation can be found here: https://github.com/tModLoader/tModLoader/blob/stable/ExampleMod/Common/UI/ExampleResourceUI/ExampleResourceBar.cs
            area = new UIElement();
            area.Left.Set(-area.Width.Pixels - 700, 1f);
            area.Top.Set(30, 0f);
            area.Width.Set(182, 0f);
            area.Height.Set(60, 0f);

            barFrame = new UIImage(TexturePaths.SpecialFrameInvisible.ToTexture2D());
            barFrame.Left.Set(22, 0f);
            barFrame.Top.Set(0, 0f);
            barFrame.Width.Set(180, 0f);
            barFrame.Height.Set(44, 0f);

            barColor = new Color(255, 255, 255);

            area.Append(barFrame);
            Append(area);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        // Here we draw our UI
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var specialPlayer = Main.LocalPlayer.GetModPlayer<SpecialPlayer>();
            if (!specialPlayer.PlayerCarriesSpecialWeapon) return;

            var colorChipPlayer = Main.LocalPlayer.GetModPlayer<ColorChipPlayer>();
            float quotient = specialPlayer.GetSpecialPercentageDisplay(); // Must be a method, property crashes for some reason
            var barColorOriginal = ColorHelper.ColorWithAlpha255(colorChipPlayer.GetColorFromChips());
            var barColorDark = ColorHelper.LerpBetweenColorsPerfect(barColorOriginal, Color.Black, 0.2f);
            barColor = barColorDark;

            float lerpAmount = 0.2f;
            if (specialPlayer.SpecialReady || specialPlayer.SpecialActivated)
            {
                lerpAmount = (float)Math.Sin(Main.time / 8) * 0.4f + 0.4f;
            }
            barColor = ColorHelper.LerpBetweenColorsPerfect(barColorDark, barColorOriginal, lerpAmount);

            Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
            hitbox.X += 12;
            hitbox.Width -= 52;
            hitbox.Y += 8;
            hitbox.Height -= 18;
            int left = hitbox.Left;
            int right = hitbox.Right;

            var yOffset = specialPlayer.GetChargeUIOffsetY();

            var barBg = TexturePaths.SpecialFrame.ToTexture2D();
            Rectangle sourceRectangle = barBg.Frame(1);
            Vector2 origin = sourceRectangle.Size() / 2;
            Main.EntitySpriteDraw(
                barBg,
                hitbox.TopLeft() + new Vector2(84, 8 + yOffset),
                sourceRectangle,
                Color.White,
                0,
                origin,
                1.02f,
                SpriteEffects.None);

            var fillEmpty = TexturePaths.SpecialFillEmpty.ToTexture2D();
            spriteBatch.Draw(
                texture: fillEmpty,
                destinationRectangle: new Rectangle(hitbox.Left - 4, hitbox.Top + 2 + (int)yOffset, fillEmpty.Width + 4, fillEmpty.Height),
                color: Color.Black);

            var fill = TexturePaths.SpecialFill.ToTexture2D();
            spriteBatch.Draw(
                texture: fill,
                destinationRectangle: new Rectangle(hitbox.Left - 4, hitbox.Top + 2 + (int)yOffset, (int)((fillEmpty.Width + 4) * quotient), fillEmpty.Height),
                color: barColor);
        }
    }
}
