using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class TriStringerProjectile : BaseProjectile
    {
        private int networkExplodeDelayBuffer = 120;

        private float delayUntilFall = 12f;
        private float fallSpeed = 0.002f;
        private float terminalVelocity = 1f;
        private bool sticking = false;
        private bool hasExploded = false;
        protected virtual bool CanStick { get => true; }

        private int finalExplosionRadius = 0;
        protected virtual int ExplosionRadius { get => 120; }
        private ExplosionDustModel explosionDustModel;

        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.extraUpdates = 30;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = ExtraUpdatesTime(120 + networkExplodeDelayBuffer);
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void AfterSpawn()
        {
            Initialize();
            finalExplosionRadius = (int)(ExplosionRadius * explosionRadiusModifier);
        }

        private float ExtraUpdatesTime(float input)
        {
            return input * Projectile.extraUpdates;
        }

        private int ExtraUpdatesTime(int input)
        {
            return input * Projectile.extraUpdates;
        }

        private void Explode()
        {
            hasExploded = true;

            Projectile.alpha = 255;
            Projectile.tileCollide = false;

            Projectile.Resize(finalExplosionRadius, finalExplosionRadius);

            explosionDustModel = new ExplosionDustModel(_dustMaxVelocity: 15f, _dustAmount: 20, _minScale: 1, _maxScale: 2, _radiusModifier: finalExplosionRadius);
            var audioModel = new PlayAudioModel("BlasterExplosionLight", _volume: 0.1f, _pitchVariance: 0.2f, _maxInstances: 10, _position: Projectile.Center);

            // Will result in endless back-and-forth if we do not check for the owner here
            if (IsThisClientTheProjectileOwner())
            {
                CreateExplosionVisual(explosionDustModel, audioModel);
                NetUpdate(ProjNetUpdateType.DustExplosion);
            }
        }

        public override void AI()
        {
            if (isFakeDestroyed) return;
            Projectile.ai[0] += 1f;

            if (!sticking)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                if (Projectile.ai[0] >= ExtraUpdatesTime(delayUntilFall))
                {
                    Projectile.velocity.Y += fallSpeed;
                }
                if (Projectile.velocity.Y > terminalVelocity)
                {
                    Projectile.velocity.Y = terminalVelocity;
                }

                Color dustColor = GenerateInkColor();
                Dust.NewDustPerfect(Position: Projectile.Center, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Vector2.Zero, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
            }
            else
            {
                if (Math.Abs(Projectile.velocity.X) > 0.0001f || Math.Abs(Projectile.velocity.Y) > 0.0001f)
                {
                    Projectile.velocity = Projectile.velocity * 0.9f;
                }

                if (!hasExploded)
                {
                    Lighting.AddLight(Projectile.Center, GenerateInkColor().ToVector3());
                }

                if (Projectile.timeLeft < ExtraUpdatesTime(networkExplodeDelayBuffer) && !hasExploded)
                {
                    if (IsThisClientTheProjectileOwner()) Explode();
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (CanStick)
            {
                if (!sticking)
                {
                    Projectile.alpha = 0;
                    PlayAudio("InkHitSplash00", volume: 0.2f, pitchVariance: 0.3f, maxInstances: 9);
                    sticking = true;
                    Projectile.tileCollide = false;
                    Projectile.velocity = oldVelocity;
                    Projectile.timeLeft = ExtraUpdatesTime(60 + networkExplodeDelayBuffer + (int)Projectile.ai[2] * 5);
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    float random = Main.rand.NextFloat(-2, 2);
                    float velX = (Projectile.velocity.X + random) * -0.5f;
                    float velY = (Projectile.velocity.Y + random) * -0.5f;
                    int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: GenerateInkColor(), Scale: Main.rand.NextFloat(0.8f, 1.6f));
                }
                Projectile.Kill();
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (sticking && !hasExploded)
            {
                if (IsThisClientTheProjectileOwner()) Explode();
                return;
            }

            if (hasExploded)
            {
                FakeDestroy();
            }
        }

        protected override void NetSendDustExplosion(BinaryWriter writer)
        {
            writer.WriteVector2(Projectile.position);
        }

        protected override void NetReceiveDustExplosion(BinaryReader reader)
        {
            Projectile.position = reader.ReadVector2();
            Explode();
            fallSpeed = 0;
            sticking = true;
        }
    }
}