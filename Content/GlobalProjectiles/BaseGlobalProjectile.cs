using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.GlobalProjectiles
{
    internal class BaseGlobalProjectile : GlobalProjectile
    {
        public bool deflected = false;
        public override bool InstancePerEntity => true;

        public override bool CanHitPlayer(Projectile projectile, Player target)
        {
            bool isFlat = target.GetModPlayer<Players.SquidPlayer>().IsFlat();
            if (isFlat)
            {
                var hitbox = new Rectangle((int)target.Left.X, (int)target.Bottom.Y - 20, target.width, 20);
                if (hitbox.Contains((int)projectile.Center.X, (int)projectile.Center.Y))
                {
                    return true;
                }

                return false;
            }

            return base.CanHitPlayer(projectile, target);
        }
    }
}
