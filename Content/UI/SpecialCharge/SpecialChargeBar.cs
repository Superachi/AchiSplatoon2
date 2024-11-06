using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
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

            barFrame = new UIImage(ModContent.Request<Texture2D>("AchiSplatoon2/Content/UI/SpecialCharge/SpecialFrame"));
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
            var weaponPlayer = Main.LocalPlayer.GetModPlayer<WeaponPlayer>();
            var colorChipPlayer = Main.LocalPlayer.GetModPlayer<ColorChipPlayer>();
            float quotient = (float)(weaponPlayer.SpecialPoints / weaponPlayer.SpecialPointsMax);
            quotient = Utils.Clamp(quotient, 0f, 1f);
            barColor = colorChipPlayer.GetColorFromChips();

            float lerpAmount = 0.2f;
            if (weaponPlayer.SpecialReady || weaponPlayer.IsSpecialActive)
            {
                lerpAmount = (float)Math.Sin(Main.time / 8) * 0.4f + 0.4f;
            }
            barColor = ColorHelper.LerpBetweenColorsPerfect(barColor, Color.White, lerpAmount);

            Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
            hitbox.X += 12;
            hitbox.Width -= 52;
            hitbox.Y += 8;
            hitbox.Height -= 18;
            int left = hitbox.Left;
            int right = hitbox.Right;

            var fillEmpty = ModContent.Request<Texture2D>("AchiSplatoon2/Content/UI/SpecialCharge/SpecialFillEmpty").Value;
            spriteBatch.Draw(
                fillEmpty,
                new Rectangle(hitbox.Left - 4, hitbox.Top + 2, fillEmpty.Width + 4, fillEmpty.Height),
                Color.White);

            var fill = ModContent.Request<Texture2D>("AchiSplatoon2/Content/UI/SpecialCharge/SpecialFill").Value;
            spriteBatch.Draw(
                fill,
                new Rectangle(hitbox.Left - 4, hitbox.Top + 2, (int)((fillEmpty.Width + 4) * quotient), fillEmpty.Height),
                barColor);
        }
    }
}
