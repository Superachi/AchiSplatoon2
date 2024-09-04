using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Projectiles.AccessoryProjectiles;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class TriStringerProjectile : BaseProjectile
    {
        private int networkExplodeDelayBuffer = 120;

        private float delayUntilFall = 12f;
        private float fallSpeed = 0.001f;

        private bool sticking = false;
        private bool hasExploded = false;
        protected virtual bool CanStick { get => true; }

        private int finalExplosionRadius = 0;
        protected virtual int ExplosionRadius { get => 120; }
        private ExplosionDustModel explosionDustModel;

        private bool countedForBurst = false;
        public bool parentFullyCharged = false;
        public bool firedWithFreshQuiver = false;

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

                Color dustColor = GenerateInkColor();
                Dust.NewDustPerfect(Position: Projectile.Center, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Vector2.Zero, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
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
            // Apply visual effect when hitting all of the burst's projectiles on the same target
            if (!sticking && !countedForBurst && parentFullyCharged)
            {
                countedForBurst = true;
                var parentProj = GetParentProjectile(parentIdentity);

                if (parentProj.ModProjectile is TriStringerCharge)
                {
                    var parentModProj = parentProj.ModProjectile as TriStringerCharge;
                    if (parentModProj.burstNPCTarget == -1)
                    {
                        parentModProj.burstNPCTarget = target.whoAmI;
                    }

                    if (parentModProj.burstNPCTarget == target.whoAmI)
                    {
                        parentModProj.burstHitCount++;
                    }

                    if (parentModProj.burstHitCount == parentModProj.burstRequiredHits)
                    {
                        TripleHitDustBurst(target.Center);
                        parentProj.Kill();

                        if (firedWithFreshQuiver)
                        {
                            CreateChildProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<FreshQuiverBlast>(), Projectile.damage, true);
                        }
                    }
                }

                base.OnHitNPC(target, hit, damageDone);
                return;
            }

            if (sticking && !hasExploded)
            {
                if (IsThisClientTheProjectileOwner()) Explode();

                base.OnHitNPC(target, hit, damageDone);
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