using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class BurstBombProjectile : BaseBombProjectile
    {
        protected override bool FallThroughPlatforms => true;

        private readonly float indirectHitDamageFalloff = 0.6f;
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;

            DrawOffsetX = -2;
            DrawOriginOffsetY = -12;
        }

        protected float ExplosionTime
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void AfterSpawn()
        {
            base.AfterSpawn();
            wormDamageReduction = true;
        }

        public override bool? CanCutTiles()
        {
            return hasExploded;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!hasExploded)
            {
                Projectile.damage = (int)(Projectile.damage * indirectHitDamageFalloff);
                hasExploded = true;
                Detonate();
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!hasExploded)
            {
                hasExploded = true;
                Detonate();
            }

            base.OnHitNPC(target, hit, damageDone);
        }

        public override void AI()
        {
            fallTimer++;

            if (!hasExploded)
            {
                Lighting.AddLight(Projectile.position, initialColor.R * brightness, initialColor.G * brightness, initialColor.B * brightness);

                // Apply air friction
                Projectile.velocity.X = Projectile.velocity.X * airFriction;

                // Rotation increased by velocity.X 
                Projectile.rotation += Projectile.velocity.X * 0.02f;

                // Apply gravity
                if (fallTimer >= delayUntilFall && !canFall)
                {
                    canFall = true;
                }

                if (canFall)
                {
                    Projectile.velocity.Y = Math.Clamp(Projectile.velocity.Y + fallSpeed, -terminalVelocity, terminalVelocity);
                }
            }
            else
            {
                ExplosionTime++;
            }

            if (ExplosionTime > 6)
            {
                Projectile.Kill();
            }
        }
    }
}
