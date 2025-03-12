using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.CustomPlayerLayers
{
    internal class BombRushBackpackLayer : PlayerDrawLayer
    {
        Texture2D? backpackTexture = null;
        float textureScale = 1f;
        float textureScaleGoal = 1f;
        bool isBombRushActive = false;

        // https://github.com/tModLoader/tModLoader/blob/44d2155c9a62fbaae86742ef2e07bc1e715e1594/ExampleMod/Common/ExamplePlayerDrawLayer.cs#L12
        public override bool IsHeadLayer => false;

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            var player = drawInfo.drawPlayer;
            isBombRushActive = player.HasBuff<BombRushBuff>();

            bool canDraw =
                player.fullRotation == 0f
                && !player.GetModPlayer<DualiePlayer>().isRolling
                && !player.GetModPlayer<SquidPlayer>().IsSquid();

            return canDraw;
        }

        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Backpacks);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            var player = drawInfo.drawPlayer;
            backpackTexture = TexturePaths.BombLauncherBackpack.ToTexture2D();

            var position = player.Center + new Vector2(player.direction * -15, -12f + player.gfxOffY) - Main.screenPosition;
            position = position.Round();

            textureScaleGoal = isBombRushActive ? 1 : 0;
            textureScale = MathHelper.Lerp(textureScale, textureScaleGoal, 0.2f);

            // Queues a drawing of a sprite. Do not use SpriteBatch in drawlayers!
            drawInfo.DrawDataCache.Add(new DrawData(
                backpackTexture, // The texture to render.
                position,
                null,
                Color.White,
                0f,
                backpackTexture.Size() * 0.5f,
                textureScale,
                player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally
            ));
        }
    }
}
