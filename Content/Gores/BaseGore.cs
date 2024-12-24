using AchiSplatoon2.Content.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Gores
{
    internal class BaseGore : ModGore
    {
        public int syncTime = 0;

        public override void OnSpawn(Gore gore, IEntitySource source)
        {
            CustomSpawn(gore);

            base.OnSpawn(gore, source);
        }

        public override bool Update(Gore gore)
        {
            gore.position += gore.velocity;
            syncTime++;

            CustomUpdate(gore);

            return false;
        }

        public override Color? GetAlpha(Gore gore, Color lightColor)
        {
            return base.GetAlpha(gore, lightColor);
        }

        protected Color GetChipColor()
        {
            return Main.LocalPlayer.GetModPlayer<ColorChipPlayer>().GetColorFromChips();
        }

        protected virtual void CustomSpawn(Gore gore)
        {
        }

        protected virtual void CustomUpdate(Gore gore)
        {
        }
    }
}
