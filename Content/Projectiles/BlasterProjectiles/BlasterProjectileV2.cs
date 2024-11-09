using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.BlasterProjectiles
{
    internal class BlasterProjectileV2 : BaseProjectile
    {
        public BaseBlaster weaponData;

        protected string explosionAirSample;
        protected string explosionTileSample;
        protected int explosionRadiusAir;
        protected int explosionRadiusTile;
        protected float explosionDelay;
        protected int damageBeforePiercing;

        private bool hasHadDirectHit = false;
        private bool hasExploded = false;
        private bool hasHitTarget = false;

        private const int stateFly = 0;
        private const int stateExplodeAir = 1;
        private const int stateExplodeTile = 2;
        private const int stateDespawn = 3;

        protected float Timer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 2;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            weaponData = (BaseBlaster)WeaponInstance;

            // Explosion radius/timing
            explosionRadiusAir = weaponData.ExplosionRadiusAir;
            explosionRadiusTile = weaponData.ExplosionRadiusTile;
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
                Projectile.damage = MultiplyProjectileDamage(FieryPaintCan.MissDamageModifier);
                explosionRadiusModifier *= FieryPaintCan.MissRadiusModifier;
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
            ExplosionDustModel e = new ExplosionDustModel(_dustMaxVelocity: 0, _dustAmount: 0, _minScale: 0, _maxScale: 0, _radiusModifier: 0);
            PlayAudioModel a;

            switch (state)
            {
                case stateFly:
                    PlayShootSound();
                    break;
                case stateExplodeAir:
                    if (!hasHadDirectHit) Projectile.damage = (int)(Projectile.damage * 0.6);

                    finalRadius = CalculateExplosionRadius(explosionRadiusAir);
                    e = new ExplosionDustModel(_dustMaxVelocity: 20, _dustAmount: 40, _minScale: 2, _maxScale: 4, _radiusModifier: finalRadius);
                    a = new PlayAudioModel(_soundPath: explosionAirSample, _volume: 0.2f, _pitchVariance: 0.1f, _maxInstances: 3);
                    CreateExplosionVisual(e, a);

                    PlayAudio(SoundID.Item167, volume: 0.4f, pitchVariance: 0.3f, maxInstances: 5, pitch: 0.5f);
                    PlayAudio(SoundID.Item38, volume: 0.4f, pitchVariance: 0.3f, maxInstances: 5, pitch: 1f);
                    PlayAudio(SoundID.Splash, volume: 0.4f, pitchVariance: 0.3f, maxInstances: 5, pitch: 1f);

                    break;
                case stateExplodeTile:
                    Projectile.damage = (int)(Projectile.damage * 0.4);

                    finalRadius = CalculateExplosionRadius(explosionRadiusTile);
                    e = new ExplosionDustModel(_dustMaxVelocity: 10, _dustAmount: 15, _minScale: 2, _maxScale: 4, _radiusModifier: finalRadius);
                    a = new PlayAudioModel(_soundPath: explosionTileSample, _volume: 0.2f, _pitchVariance: 0.1f, _maxInstances: 3);
                    CreateExplosionVisual(e, a);

                    break;
                case stateDespawn:
                    Projectile.Kill();
                    break;
            }

            if (state == stateExplodeAir || state == stateExplodeTile)
            {
                Projectile.penetrate = -1;
                wormDamageReduction = true;

                Projectile.tileCollide = false;
                Timer = 0;
                Projectile.position -= Projectile.velocity;
                Projectile.velocity = Vector2.Zero;
                Projectile.Resize(finalRadius, finalRadius);
                hasExploded = true;
            }
        }

        public override void AI()
        {
            Timer++;

            switch (state)
            {
                case stateFly:
                    ProjectileDustHelper.BlasterDustTrail(this);

                    if (Timer >= explosionDelay * FrameSpeed())
                    {
                        SetState(stateExplodeAir);
                    }
                    break;
                case stateExplodeAir:
                case stateExplodeTile:
                    if (Timer >= 6 * FrameSpeed())
                    {
                        SetState(stateDespawn);
                    }
                    break;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (state != stateExplodeTile) hasHitTarget = true;

            if (!hasExploded && !hasHadDirectHit)
            {
                hasHadDirectHit = true;
                DirectHitDustBurst(target.Center);
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

        protected override void AfterKill(int timeLeft)
        {
            if (IsThisClientTheProjectileOwner())
            {
                var accMP = GetOwner().GetModPlayer<AccessoryPlayer>();
                if (accMP.hasFieryPaintCan) accMP.SetBlasterBuff(hasHitTarget);
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return CanHitNPCWithLineOfSight(target);
        }
    }
}
