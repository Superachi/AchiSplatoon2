using AchiSplatoon2.Content.Players;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Projectiles.DualieProjectiles
{
    internal class PainterDualieShotProjectile : DualieShotProjectile
    {
        public override void AfterSpawn()
        {
            base.AfterSpawn();
            var colorMP = GetOwner().GetModPlayer<InkColorPlayer>();
            colorMP.IncreaseHueBy(10, out float hue);
            colorOverride = colorMP.currentColor;
        }
    }
}
