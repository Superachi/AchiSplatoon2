using Terraria;
using Microsoft.Xna.Framework;
using System;
using AchiSplatoon2.Helpers;

namespace AchiSplatoon2.Content.Gores
{
    internal class MusicNoteGore : BaseGore
    {
        protected override void CustomSpawn(Gore gore)
        {
            gore.numFrames = 4;
            gore.frame = (byte)Main.rand.Next(4);
            gore.frameCounter = (byte)Main.rand.Next(4);
        }

        protected override void CustomUpdate(Gore gore)
        {
            gore.position += gore.velocity;
            gore.alpha += 4;
            gore.rotation = (float)Math.Sin(syncTime / 56f);
            
            if (gore.scale < 1)
            {
                gore.scale += 0.05f;
            }

            if (gore.alpha > 255)
            {
                gore.active = false;
            }
        }

        public override Color? GetAlpha(Gore gore, Color lightColor)
        {
            var alphaMod = (float)(255 - gore.alpha) / 255f;
            var newColor = ColorHelper.ColorWithAlpha255(GetChipColor()) * alphaMod;
            return newColor;
        }
    }
}
