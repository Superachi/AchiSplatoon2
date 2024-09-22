using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using Terraria;
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
            weaponData = WeaponInstance as BaseBlaster;

            // Explosion radius/timing
            explosionRadiusAir = weaponData.ExplosionRadiusAir;
            explosionRadiusTile = weaponData.ExplosionRadiusTile;
            explosionDelay = weaponData.ExplosionDelayInit;

            // Audio
            shootSample = weaponData.ShootSample;
            explosionAirSample = weaponData.ExplosionBigSample;
            explosionTileSample = weaponData.ExplosionSmallSample;
        }

        public override void AfterSpawn()
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

        protected override void PlayShootSound()
        {
            PlayAudio(shootSample, volume: 0.3f, pitchVariance: 0.1f, maxInstances: 3);
        }

        private void EmitTrailInkDust(float dustMaxVelocity = 1, int amount = 1, float minScale = 0.5f, float maxScale = 1f, Vector2? position = null)
        {
            var pos = Projectile.Center;
            if (position != null) { position = pos; }
            for (int i = 0; i < amount; i++)
            {
                Color dustColor = GenerateInkColor();
                Dust.NewDustPerfect(
                    pos + Main.rand.NextVector2Circular(10, 10),
                    ModContent.DustType<SplatterBulletLastingDust>(),
                    Main.rand.NextVector2Circular(-dustMaxVelocity, dustMaxVelocity),
                    255,
                    dustColor,
                    Main.rand.NextFloat(minScale, maxScale)
                );
            }
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
                    break;
                case stateExplodeTile:
                    Projectile.damage = (int)(Projectile.damage * 0.4);

                    finalRadius = CalculateExplosionRadius(explosionRadiusTile);
                    e = new ExplosionDustModel(_dustMaxVelocity: 10, _dustAmount: 15, _minScale: 1, _maxScale: 2, _radiusModifier: finalRadius);
                    a = new PlayAudioModel(_soundPath: explosionTileSample, _volume: 0.1f, _pitchVariance: 0.1f, _maxInstances: 3);
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
                    EmitTrailInkDust(dustMaxVelocity: 1f, amount: 3, minScale: 0.5f, maxScale: 2);

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

        public override void OnKill(int timeLeft)
        {
            if (IsThisClientTheProjectileOwner())
            {
                var accMP = GetOwner().GetModPlayer<InkAccessoryPlayer>();
                if (accMP.hasFieryPaintCan) accMP.SetBlasterBuff(hasHitTarget);
            }

            base.OnKill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return CanHitNPCWithLineOfSight(target);
        }
    }
}
