using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.BrushProjectiles
{
    internal class DesertBrushSwingProjectile : BrushSwingProjectile
    {
        public BaseProjectile? ChildProjectile = null;

        protected override void AddedRollEffect()
        {
            if (ChildProjectile == null)
            {
                var p = CreateChildProjectile<DesertBrushDashProjectile>(Owner.Center, Vector2.Zero, Projectile.damage * 6, triggerSpawnMethods: true);
                ChildProjectile = p;
            }
        }
    }
}
