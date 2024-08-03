using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Projectiles.ProjectileVisuals;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.ChargerProjectiles
{
    internal class StarfishedChargerBlast : BaseProjectile
    {
        protected string explosionSample = "BlasterExplosion";
        private int baseRadius = 100;
        private bool hasExploded = false;

        public float pitchAdd = 0f;
        public int delayUntilBlast = 40;
        public int npcTarget = -1;
        private int blastTimer = 0;

        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.knockBack = 5;
        }

        public override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            enablePierceDamagefalloff = false;
        }

        public override void AI()
        {
            NPC target = Main.npc[npcTarget];
            if (target.life <= 0)
            {
                Projectile.Kill();
                return;
            }

            blastTimer++;
            if (blastTimer >= delayUntilBlast && !hasExploded)
            {
                if (IsThisClientTheProjectileOwner())
                {
                    Projectile.friendly = true;
                    
                    var finalRadius = (int)(baseRadius * explosionRadiusModifier);
                    Projectile.Resize(finalRadius, finalRadius);
                    Projectile.Center = target.Center;

                    var e = new ExplosionDustModel(_dustMaxVelocity: 28, _dustAmount: 10, _minScale: 2, _maxScale: 4, _radiusModifier: finalRadius);
                    var a = new PlayAudioModel(_soundPath: explosionSample, _volume: 0.3f, _pitchVariance: 0.1f, _maxInstances: 3, _pitch: -0.4f + pitchAdd, _position: Projectile.Center);
                    CreateExplosionVisual(e, a);

                    hasExploded = true;
                    Projectile.timeLeft = 6;
                }
            } else
            {
                if (Main.rand.NextBool(4))
                {
                    var dust = Dust.NewDustPerfect(
                        Position: target.Center,
                        Type: 264,
                        Velocity: Main.rand.NextVector2Circular(3, 3),
                        newColor: GenerateInkColor(),
                        Scale: Main.rand.NextFloat(0.8f, 1.6f));
                    dust.noGravity = true;
                }
            }
        }
    }
}
