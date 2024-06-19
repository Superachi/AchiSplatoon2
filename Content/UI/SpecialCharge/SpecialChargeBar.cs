using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SteelSeries.GameSense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace AchiSplatoon2.Content.UI.SpecialCharge
{
    internal class SpecialChargeBar : UIState
    {
        private UIText text;
        private UIElement area;
        private UIImage barFrame;
        private Color gradientB;

        public override void OnInitialize()
        {
            // Explanation can be found here: https://github.com/tModLoader/tModLoader/blob/stable/ExampleMod/Common/UI/ExampleResourceUI/ExampleResourceBar.cs
            area = new UIElement();
            area.Left.Set(-area.Width.Pixels - 600, 1f);
            area.Top.Set(30, 0f);
            area.Width.Set(182, 0f);
            area.Height.Set(60, 0f);

            barFrame = new UIImage(ModContent.Request<Texture2D>("AchiSplatoon2/Content/UI/SpecialCharge/SpecialFrame"));
            barFrame.Left.Set(22, 0f);
            barFrame.Top.Set(0, 0f);
            barFrame.Width.Set(180, 0f);
            barFrame.Height.Set(44, 0f);

            text = new UIText("0/0", 0.8f);
            text.Width.Set(180, 0f);
            text.Height.Set(44, 0f);
            text.Top.Set(40, 0f);
            text.Left.Set(0, 0f);

            gradientB = new Color(187, 91, 201); // A light purple

            area.Append(text);
            area.Append(barFrame);
            Append(area);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //var item = Main.LocalPlayer.HeldItem.ModItem;
            //var itemType = item.GetType();
            //if (!itemType.IsAssignableFrom(typeof(BaseWeapon)))
            //    return;

            base.Draw(spriteBatch);
        }

        // Here we draw our UI
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();

            float quotient = (float)(modPlayer.SpecialPoints / modPlayer.SpecialPointsMax);
            quotient = Utils.Clamp(quotient, 0f, 1f);

            Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
            hitbox.X += 12;
            hitbox.Width -= 24;
            hitbox.Y += 8;
            hitbox.Height -= 16;

            int left = hitbox.Left;
            int right = hitbox.Right;
            int steps = (int)((right - left) * quotient);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(hitbox.Left, hitbox.Y, (int)(hitbox.Width * quotient), hitbox.Height), gradientB);
        }

        public override void Update(GameTime gameTime)
        {
            //var item = Main.LocalPlayer.HeldItem.ModItem;
            //var itemType = item.GetType();
            //if (!itemType.IsAssignableFrom(typeof(BaseWeapon)))
            //    return;

            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();

            //// Setting the text per tick to update and show our resource values.
            int percentage = (int)(modPlayer.SpecialPoints / modPlayer.SpecialPointsMax * 100);
            text.SetText($"{percentage}%");
            base.Update(gameTime);
        }
    }
}
