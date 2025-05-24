using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.BlasterProjectiles
{
    internal class BlasterProjectile : BaseProjectile
    {
        protected SoundStyle? explosionAirSample;
        protected SoundStyle? explosionTileSample;

        protected int explosionRadiusAir;
        protected float tileExplosionRadiusModifier = 0.6f;
        protected float explosionDelay;
        protected int damageBeforePiercing;

        private bool hasHadDirectHit = false;

        private const int stateFly = 0;
        private const int stateExplodeAir = 1;
        private const int stateExplodeTile = 2;

        private readonly List<int> hitTargets = new List<int>();

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 2;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }

        public void ModifyExplosionRadius(float modifier)
        {
            explosionRadiusAir = (int)(explosionRadiusAir * modifier);
        }

        public void ModifyExplosionDelay(float modifier)
        {
            explosionDelay = (int)(explosionDelay * modifier);
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = (BaseBlaster)WeaponInstance;

            // Explosion radius/timing
            explosionRadiusAir = weaponData.ExplosionRadiusAir;
            explosionDelay = weaponData.ExplosionDelayInit;

            // Audio
            shootSample = weaponData.ShootSample;
            explosionAirSample = weaponData.ExplosionBigSample;
            explosionTileSample = weaponData.ExplosionSmallSample;
        }

        protected override void AfterSpawn()
        {
            // Mechanics
            Initialize();
            ApplyWeaponInstanceData();
            enablePierceDamagefalloff = false;

            if (GetOwner().HasBuff<BigBlastBuff>())
            {
                if (IsThisClientTheProjectileOwner())
                {
                    Projectile.damage = MultiplyProjectileDamage(FieryPaintCan.MissDamageModifier);
                    explosionRadiusModifier *= FieryPaintCan.MissRadiusModifier;
                }
            }

            damageBeforePiercing = Projectile.damage;
            SetState(stateFly);
        }

        protected override void AdjustVariablesOnShoot()
        {
            if (IsThisClientTheProjectileOwner())
            {
                Projectile.velocity *= 0.4f;
            }

            Projectile.extraUpdates *= 3;
            Projectile.timeLeft *= 3;
        }

        protected override void CreateDustOnSpawn()
        {
            ProjectileDustHelper.BlasterSpawnVisual(this);
        }

        protected override void PlayShootSound()
        {
            PlayAudio(shootSample, volume: 0.4f, pitchVariance: 0.2f, maxInstances: 5, pitch: -0.3f);
            PlayAudio(SoundID.Item38, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 5, pitch: -0.5f);
            PlayAudio(SoundID.Item45, volume: 0.3f, pitchVariance: 0.2f, maxInstances: 5, pitch: 1f);
        }

        private int CalculateExplosionRadius(int baseRadius)
        {
            return (int)(baseRadius * explosionRadiusModifier);
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);

            int finalRadius = 0;
            BlasterExplosion p;

            switch (state)
            {
                case stateFly:
                    PlayShootSound();
                    break;
                case stateExplodeAir:
                    if (!hasHadDirectHit) Projectile.damage = (int)(Projectile.damage * 0.6);
                    finalRadius = CalculateExplosionRadius(explosionRadiusAir);

                    if (explosionAirSample != null)
                    {
                        PlayAudio((SoundStyle)explosionAirSample, volume: 0.2f, pitchVariance: 0.1f, maxInstances: 5, pitch: 0f);
                    }
                    PlayAudio(SoundID.Item167, volume: 0.4f, pitchVariance: 0.3f, maxInstances: 5, pitch: 0.5f);
                    PlayAudio(SoundID.Item38, volume: 0.4f, pitchVariance: 0.3f, maxInstances: 5, pitch: 1f);
                    PlayAudio(SoundID.Splash, volume: 0.4f, pitchVariance: 0.3f, maxInstances: 5, pitch: 1f);

                    break;
                case stateExplodeTile:
                    Projectile.damage = (int)(Projectile.damage * 0.4);
                    finalRadius = CalculateExplosionRadius((int)(explosionRadiusAir * tileExplosionRadiusModifier));

                    if (explosionTileSample != null)
                    {
                        PlayAudio((SoundStyle)explosionTileSample, volume: 0.3f, pitchVariance: 0.1f, maxInstances: 5, pitch: 0.5f);
                    }

                    break;
            }

            if (state == stateExplodeAir || state == stateExplodeTile)
            {
                p = CreateChildProjectile<BlasterExplosion>(
                    Projectile.Center,
                    Vector2.Zero,
                    Projectile.damage,
                    triggerSpawnMethods: false);

                p.SetProperties(finalRadius, hitTargets);
                p.RunSpawnMethods();

                Projectile.Kill();
            }
        }

        public override void AI()
        {
            if (state == stateFly)
            {
                ProjectileDustHelper.BlasterDustTrail(this);

                if (timeSpentAlive >= explosionDelay * FrameSpeed())
                {
                    SetState(stateExplodeAir);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hitTargets.Add(target.whoAmI);

            if (!hasHadDirectHit)
            {
                hasHadDirectHit = true;

                if (target.life > 0)
                {
                    WeakDirectDustBurst(target.Center);
                }
                else
                {
                    DirectHitDustBurst(target.Center);
                }
            }

            if (state == stateFly && Projectile.penetrate <= 1)
            {
                SetState(stateExplodeAir);
            }

            base.OnHitNPC(target, hit, damageDone);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (state == stateFly)
            {
                SetState(stateExplodeTile);
            }

            return false;
        }
    }
}
