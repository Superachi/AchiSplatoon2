using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Newtonsoft.Json;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class BlasterProjectile : BaseProjectile
    {
        private const int addedUpdate = 2;

        private ExplosionDustModel explosionDustModel;

        protected string explosionBigSample;
        protected string explosionSmallSample;

        protected int explosionRadiusAir;
        private int finalExplosionRadiusAir;

        protected int explosionRadiusTile;
        private int finalExplosionRadiusTile;

        private float explosionDelay;
        private const float explosionTime = 6f;
        protected float explosionDelayInit;

        protected int damageBeforePiercing;
        private bool hasHadDirectHit = false;
        private bool hasExploded = false;
        private float directDamageModifier = 1f;

        public override void SetDefaults()
        {
            Projectile.extraUpdates = addedUpdate;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Initialize();
            damageBeforePiercing = Projectile.damage;
            Blaster weaponData = (Blaster)weaponSource;

            explosionRadiusAir = weaponData.ExplosionRadiusAir;
            explosionRadiusTile = weaponData.ExplosionRadiusTile;
            explosionDelayInit = weaponData.ExplosionDelayInit;
            explosionBigSample = weaponData.ExplosionBigSample;
            explosionSmallSample = weaponData.ExplosionSmallSample;
            shootSample = weaponData.ShootSample;

            explosionDelay = explosionDelayInit * addedUpdate;
            finalExplosionRadiusAir = (int)(explosionRadiusAir * explosionRadiusModifier);
            finalExplosionRadiusTile = (int)(explosionRadiusTile * explosionRadiusModifier);

            PlayShootSound();
            EmitShotBurstDust();
        }

        private void EmitShotBurstDust()
        {
            for (int i = 0; i < 15; i++)
            {
                Color dustColor = GenerateInkColor();

                float random = Main.rand.NextFloat(-5, 5);
                float velX = ((Projectile.velocity.X + random) * 0.5f);
                float velY = ((Projectile.velocity.Y + random) * 0.5f);

                Dust.NewDust(Projectile.position, 1, 1, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
            }
        }

        private void EmitTrailInkDust(float dustMaxVelocity = 1, int amount = 1, float minScale = 0.5f, float maxScale = 1f)
        {
            for (int i = 0; i < amount; i++)
            {
                Color dustColor = GenerateInkColor();
                Dust.NewDustPerfect(Projectile.position, ModContent.DustType<BlasterTrailDust>(),
                    new Vector2(Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity), Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity)),
                    255, dustColor, Main.rand.NextFloat(minScale, maxScale));
            }
        }

        private void Explode(PlayAudioModel audio)
        {
            hasExploded = true;
            Projectile.penetrate = -1;
            Projectile.damage = damageBeforePiercing;
            CreateExplosionVisual(explosionDustModel, audio);
        }

        private void ExplodeBig()
        {
            explosionDustModel = new ExplosionDustModel(_dustMaxVelocity: 20, _dustAmount: 40, _minScale: 2, _maxScale: 4, _radiusModifier: finalExplosionRadiusAir);
            var audio = new PlayAudioModel(_soundPath: explosionBigSample, _volume: 0.2f, _pitchVariance: 0.1f, _maxInstances: 3);
            Explode(audio);

            // Gameplay
            if (Projectile.owner == Main.myPlayer)
            {
                if (!hasHadDirectHit)
                {
                    Projectile.damage /= 2;
                }
                Projectile.tileCollide = false;
                Projectile.Resize(finalExplosionRadiusAir, finalExplosionRadiusAir);
                Projectile.velocity = Vector2.Zero;
            }
        }

        private void ExplodeSmall()
        {
            explosionDustModel = new ExplosionDustModel(_dustMaxVelocity: 10, _dustAmount: 15, _minScale: 1, _maxScale: 2, _radiusModifier: finalExplosionRadiusTile);
            var audio = new PlayAudioModel(_soundPath: explosionSmallSample, _volume: 0.1f, _pitchVariance: 0.1f, _maxInstances: 3);
            Explode(audio);

            // Gameplay
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.damage /= 2;
                Projectile.tileCollide = false;
                Projectile.Resize(finalExplosionRadiusTile, finalExplosionRadiusTile);
                Projectile.velocity = Vector2.Zero;
            }
        }

        protected override void AdvanceState()
        {
            state++;
            Projectile.ai[0] = 0;
        }

        protected override void SetState(int stateId)
        {
            state = stateId;
            Projectile.ai[0] = 0;
        }

        public override void AI()
        {
            switch (state)
            {
                case 0:
                    EmitTrailInkDust(dustMaxVelocity: 0.2f, amount: 4, minScale: 1, maxScale: 3);
                    NetUpdate(ProjNetUpdateType.EveryFrame);

                    if (Projectile.ai[0] >= explosionDelay)
                    {
                        ExplodeBig();
                        AdvanceState();
                    }
                    break;
                case 1:
                case 2:
                    if (Projectile.ai[0] >= explosionTime)
                    {
                        Projectile.Kill();
                    }
                    break;
            }

            Projectile.ai[0] += 1f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!hasExploded && !hasHadDirectHit)
            {
                hasHadDirectHit = true;
                DirectHitDustBurst(target.Center);
            }

            Projectile.damage = (int)(Projectile.damage * directDamageModifier);

            if (state == 0 && Projectile.penetrate <= 1)
            {
                ExplodeBig();
                AdvanceState();
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (state == 0)
            {
                ExplodeSmall();
                SetState(2);
            }
            return false;
        }

        private void PlayShootSound()
        {
            PlayAudio(shootSample, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 3);
        }

        // Netcode
        protected override void NetReceiveInitialize(BinaryReader reader)
        {
            base.NetReceiveInitialize(reader);
            PlayShootSound();
        }

        protected override void NetReceiveEveryFrame(BinaryReader reader)
        {
            if (state != 0) return;
            EmitTrailInkDust(dustMaxVelocity: 0.2f, amount: 4, minScale: 1, maxScale: 3);
        }
    }
}