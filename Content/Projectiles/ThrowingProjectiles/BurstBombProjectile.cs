using AchiSplatoon2.Content.Items.Weapons.Throwing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class BurstBombProjectile : BaseBombProjectile
    {
        private float indirectHitDamageFalloff = 0.5f;
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
            if (!hasExploded)
            {
                Lighting.AddLight(Projectile.position, glowColor.R * brightness, glowColor.G * brightness, glowColor.B * brightness);

                // Apply air friction
                Projectile.velocity.X = Projectile.velocity.X * airFriction;

                // Rotation increased by velocity.X 
                Projectile.rotation += Projectile.velocity.X * 0.02f;

                // Apply gravity
                Projectile.velocity.Y = Math.Clamp(Projectile.velocity.Y + 0.3f, -terminalVelocity, terminalVelocity);
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
