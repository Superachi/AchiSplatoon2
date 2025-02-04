using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.AccessoryProjectiles;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class TriStringerProjectile : BaseProjectile
    {
        public bool canStick = false;
        private readonly int networkExplodeDelayBuffer = 120;

        private readonly float delayUntilFall = 8f;
        private float fallSpeed;

        protected bool sticking = false;
        protected bool hasExploded = false;

        protected virtual int ExplosionRadius { get => 120; }
        private readonly ExplosionDustModel explosionDustModel;
        protected int finalExplosionRadius = 0;

        private bool countedForBurst = false;
        public bool parentFullyCharged = false;
        public bool firedWithFreshQuiver = false;

        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.extraUpdates = 32;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = ExtraUpdatesTime(120 + networkExplodeDelayBuffer);
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            fallSpeed = 0.003f;
            finalExplosionRadius = (int)(ExplosionRadius * explosionRadiusModifier);
        }

        protected override void AdjustVariablesOnShoot()
        {
            if (IsThisClientTheProjectileOwner())
            {
                Projectile.velocity *= 0.5f;
            }

            Projectile.extraUpdates *= 2;
            Projectile.timeLeft *= 2;
            fallSpeed *= 0.15f;
        }

        protected override void CreateDustOnSpawn()
        {
        }

        private float ExtraUpdatesTime(float input)
        {
            return input * Projectile.extraUpdates;
        }

        private int ExtraUpdatesTime(int input)
        {
            return input * Projectile.extraUpdates;
        }

        protected virtual void Explode()
        {
            hasExploded = true;

            Projectile.alpha = 255;
            Projectile.tileCollide = false;

            var audioModel = new PlayAudioModel(SoundPaths.BlasterExplosionLight, _volume: 0.1f, _pitchVariance: 0.2f, _maxInstances: 10, _position: Projectile.Center);

            if (IsThisClientTheProjectileOwner())
            {
                BlastProjectile p = CreateChildProjectile<BlastProjectile>(Projectile.Center, Vector2.Zero, Projectile.damage, false);
                p.SetProperties(finalExplosionRadius, audioModel);
                p.RunSpawnMethods();
                Projectile.Kill();
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

                DustHelper.NewDust(
                    position: Projectile.Center,
                    dustType: ModContent.DustType<SplatterBulletDust>(),
                    velocity: Vector2.Zero,
                    color: CurrentColor,
                    scale: 1f,
                    data: new (scaleIncrement: -0.2f));

                if (Main.rand.NextBool(40))
                {
                    DustHelper.NewDropletDust(
                        position: Projectile.Center,
                        velocity: Vector2.Zero,
                        color: CurrentColor,
                        scale: 1f);
                }
            }
            else
            {
                if (Math.Abs(Projectile.velocity.X) > float.Epsilon || Math.Abs(Projectile.velocity.Y) > float.Epsilon)
                {
                    Projectile.velocity = Projectile.velocity * 0.1f;
                }

                if (!hasExploded)
                {
                    Lighting.AddLight(Projectile.Center, CurrentColor.ToVector3());
                }

                if (Projectile.timeLeft < ExtraUpdatesTime(networkExplodeDelayBuffer) && !hasExploded)
                {
                    if (IsThisClientTheProjectileOwner()) Explode();
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (canStick)
            {
                if (!sticking)
                {
                    Projectile.alpha = 0;
                    PlayAudio(SoundPaths.InkHitSplash00.ToSoundStyle(), volume: 0.2f, pitchVariance: 0.3f, maxInstances: 9);
                    sticking = true;
                    Projectile.friendly = false;
                    Projectile.tileCollide = false;
                    Projectile.position -= Projectile.velocity;
                    Projectile.velocity = oldVelocity;
                    Projectile.timeLeft = ExtraUpdatesTime(60 + networkExplodeDelayBuffer + (int)Projectile.ai[2] * 5);
                }
            }
            else
            {
                ProjectileDustHelper.ShooterTileCollideVisual(this);
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

                    if (parentModProj.burstRequiredHits > 1)
                    {
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
    }
}