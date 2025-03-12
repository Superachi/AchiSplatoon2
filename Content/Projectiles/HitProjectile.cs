using System;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class HitProjectile : BaseProjectile
    {
        public int targetToHit = -1;
        public int immuneTime = 0;

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
            Projectile.width = 1;
            Projectile.height = 1;
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            Projectile.penetrate = 1;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI != targetToHit)
            {
                return false;
            }

            return base.CanHitNPC(target);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride = Math.Sign(target.position.X - Owner.position.X);
            if (immuneTime > 0)
            {
                target.immune[Owner.whoAmI] = immuneTime;
            }

            base.ModifyHitNPC(target, ref modifiers);
        }
    }
}
