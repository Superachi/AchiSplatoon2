using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.GlobalNPCs
{
    internal class MarkBuffGlobalNPC : BaseGlobalNPC
    {
        private float _alphaMod;
        private float _widthMod;
        private Texture2D _dotTexture;

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            _dotTexture = TexturePaths.MarkedDot.ToTexture2D();
            base.OnSpawn(npc, source);
        }

        private bool IsMarked(NPC npc)
        {
            return npc.HasBuff(ModContent.BuffType<Buffs.Debuffs.MarkedBuff>());
        }

        public void ApplyEffect()
        {
            _alphaMod = 0f;
            _widthMod = 10f;
        }

        public override void AI(NPC npc)
        {
            var chipColor = Main.LocalPlayer.GetModPlayer<ColorChipPlayer>().GetColorFromChips();
            
            if (IsMarked(npc))
            {
                if (_alphaMod < 1f) _alphaMod += 0.25f;
                _widthMod = MathHelper.Lerp(_widthMod, 1f, 0.3f);

                var lineCol = chipColor * 0.6f * _alphaMod;
                var lightVal = 0.0015f;
                Lighting.AddLight(npc.Center, lineCol.R * lightVal, lineCol.G * lightVal, lineCol.B * lightVal);

                if (Main.rand.NextBool(2) && (int)Main.time % 20 == 0)
                {
                    var dust = DustHelper.NewDust(
                        npc.Center + Main.rand.NextVector2Circular(npc.width / 2, npc.height / 2),
                        DustID.PortalBolt,
                        new Vector2(0, Main.rand.NextFloat(-2, -5)),
                        chipColor,
                        Main.rand.NextFloat(1f, 2f));

                    dust.noLight = true;
                }
            }
            else
            {
                if (_alphaMod > 0f) _alphaMod -= 0.1f;
                _widthMod = MathHelper.Lerp(_widthMod, 0f, 0.1f);
            }

            _alphaMod = MathHelper.Clamp(_alphaMod, 0f, 1f);
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PostDraw(npc, spriteBatch, screenPos, drawColor);

            var sinMult = 0.75f + (float)Math.Sin(Main.time / 16) / 4;

            if (_alphaMod > 0f)
            {
                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                var player = Main.LocalPlayer;
                float linewidth = 2f * _widthMod;
                var chipColor = Main.LocalPlayer.GetModPlayer<ColorChipPlayer>().GetColorFromChips();
                var lineCol = ColorHelper.ColorWithAlpha255(chipColor) * _alphaMod * sinMult;

                Utils.DrawLine(
                    spriteBatch,
                    player.Center,
                    npc.Center,
                    new Color(lineCol.R, lineCol.G, lineCol.B, 0),
                    new Color(lineCol.R, lineCol.G, lineCol.B, 80),
                    linewidth);

                Main.EntitySpriteDraw(
                    texture: TexturePaths.MarkedDot.ToTexture2D(),
                    position: npc.Center - Main.screenPosition,
                    sourceRectangle: null,
                    color: lineCol,
                    rotation: 0,
                    origin: TexturePaths.MarkedDot.ToTexture2D().Size() / 2,
                    scale: _alphaMod,
                    effects: SpriteEffects.None);

                Main.EntitySpriteDraw(
                    texture: TexturePaths.Glow100x.ToTexture2D(),
                    position: npc.Center - Main.screenPosition,
                    sourceRectangle: null,
                    color: lineCol * 0.5f,
                    rotation: 0,
                    origin: TexturePaths.Glow100x.ToTexture2D().Size() / 2,
                    scale: _alphaMod,
                    effects: SpriteEffects.None);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
    }
}
