using AchiSplatoon2.Content.Dusts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Content.GlobalProjectiles;
using Terraria.ID;
using static Terraria.GameContent.Animations.IL_Actions.Sprites;

namespace AchiSplatoon2.Content.Projectiles.BrellaProjectiles
{
    internal class BrellaMeleeProjectile : BaseProjectile
    {
        private Vector2 shieldAngle;
        private float shieldAngleOffsetMult = 30f;

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 54;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
        }

        public override void AfterSpawn()
        {
            base.AfterSpawn();
            Initialize(isDissolvable: false);
            enablePierceDamagefalloff = false;

            Projectile.timeLeft = GetOwner().itemTimeMax;
            shieldAngle = Projectile.velocity * shieldAngleOffsetMult;
            Projectile.velocity = Vector2.Zero;
        }

        public override void AI()
        {
            Player owner = GetOwner();
            SyncProjectilePosWithPlayer(owner, shieldAngle.X - Projectile.width / 2, shieldAngle.Y - Projectile.height / 2);

            if (!IsThisClientTheProjectileOwner()) return;
            Rectangle projectileRect = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
            Projectile deflectedProj = DeflectProjectileWithinRectangle(projectileRect);

            if (deflectedProj != null)
            {
                PlayAudio("Brellas/BrellaDeflect", pitchVariance: 0.4f, maxInstances: 5);

                // Ink
                for (int i = 0; i < 15; i++)
                {
                    Color dustColor = GenerateInkColor();
                    var dust = Dust.NewDustPerfect(deflectedProj.Center, ModContent.DustType<SplatterDropletDust>(),
                        Vector2.Normalize(deflectedProj.velocity) * 8 + Main.rand.NextVector2Circular(3, 3),
                        255, dustColor, Main.rand.NextFloat(0.5f, 1f));
                }

                // Firework
                for (int i = 0; i < 15; i++)
                {
                    Color dustColor = GenerateInkColor();
                    var dust = Dust.NewDustPerfect(deflectedProj.Center, DustID.FireworksRGB,
                        Vector2.Normalize(deflectedProj.velocity) * 8 + Main.rand.NextVector2Circular(3, 3),
                        255, dustColor, Main.rand.NextFloat(0.5f, 1f));
                }
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            var p = GetOwner();
            if (Collision.CanHitLine(p.Center, 1, 1, target.Center, 1, 1))
            {
                if (!target.friendly) return true;
            }
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            Projectile.knockBack += 5;
            modifiers.HitDirectionOverride = GetOwner().direction;
        }
    }
}
