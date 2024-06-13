using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Threading;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class BlasterProjectile : BaseProjectile
    {
        private const int addedUpdate = 2;
        private int state = 0;

        protected virtual string ShootSample { get => "BlasterShoot"; }
        protected virtual string ExplosionBigSample { get => "BlasterExplosion"; }
        protected virtual string ExplosionSmallSample { get => "BlasterExplosionLight"; }

        private int finalExplosionRadiusAir = 0;
        protected virtual int ExplosionRadiusAir { get => 240; }

        private int finalExplosionRadiusTile = 0;
        protected virtual int ExplosionRadiusTile { get => 160; }
        protected virtual float ExplosionDelayInit { get => 20f; }

        private float explosionDelay = 0f;
        private const float explosionTime = 6f;

        public override void SetDefaults()
        {
            explosionDelay = ExplosionDelayInit * addedUpdate;
            Projectile.extraUpdates = addedUpdate;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1; // Required for the 'AoE' to be able to hit multiple enemies
            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Initialize();
            finalExplosionRadiusAir = (int)(ExplosionRadiusAir * explosionRadiusModifier);
            finalExplosionRadiusTile = (int)(ExplosionRadiusTile * explosionRadiusModifier);
            PlayAudio(ShootSample, volume: 0.3f, pitchVariance: 0.1f, maxInstances: 3);
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

        private void EmitBurstDust(float dustMaxVelocity = 1, int amount = 1, float minScale = 0.5f, float maxScale = 1f, float radiusModifier = 100f)
        {
            float radiusMult = radiusModifier / 160;
            amount = Convert.ToInt32(amount * radiusMult);

            // Ink
            for (int i = 0; i < amount; i++)
            {
                Color dustColor = GenerateInkColor();

                var dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<BlasterExplosionDust>(),
                    new Vector2(Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity), Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity)),
                    255, dustColor, Main.rand.NextFloat(minScale, maxScale));
                dust.velocity *= radiusMult;
            }

            // Firework
            for (int i = 0; i < amount / 2; i++)
            {
                Color dustColor = GenerateInkColor();
                var dust = Dust.NewDustPerfect(Projectile.Center, DustID.FireworksRGB,
                    new Vector2(Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity), Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity)),
                    255, dustColor);
                dust.velocity *= radiusMult / 2;
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

        private void ExplodeBig()
        {
            // Audio
            PlayAudio(ExplosionBigSample, volume: 0.2f, pitchVariance: 0.1f, maxInstances: 3);

            // Gameplay
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.tileCollide = false;
                Projectile.Resize(finalExplosionRadiusAir, finalExplosionRadiusAir);
                Projectile.velocity = Vector2.Zero;
            }

            // Visual
            EmitBurstDust(dustMaxVelocity: 20, amount: 40, minScale: 2, maxScale: 4, radiusModifier: finalExplosionRadiusAir);
        }

        private void ExplodeSmall()
        {
            // Audio
            PlayAudio(ExplosionSmallSample, volume: 0.1f, pitchVariance: 0.1f, maxInstances: 3);

            // Gameplay
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.damage /= 2;
                Projectile.tileCollide = false;
                Projectile.Resize(finalExplosionRadiusTile, finalExplosionRadiusTile);
                Projectile.velocity = Vector2.Zero;
            }

            // Visual
            EmitBurstDust(dustMaxVelocity: 10, amount: 15, minScale: 1, maxScale: 2, radiusModifier: finalExplosionRadiusTile);
        }

        private void AdvanceState()
        {
            state++;
            Projectile.ai[0] = 0;
        }

        private void SetState(int stateId)
        {
            state = stateId;
            Projectile.ai[0] = 0;
        }

        public override void AI()
        {
            // Face direction
            Projectile.rotation = Projectile.velocity.ToRotation();

            switch (state)
            {
                case 0:
                    EmitTrailInkDust(dustMaxVelocity: 0.2f, amount: 4, minScale: 1, maxScale: 3);

                    if (Projectile.ai[0] >= explosionDelay)
                    {
                        ExplodeBig();
                        AdvanceState();
                    }
                    break;
                case 1:
                    if (Projectile.ai[0] >= explosionTime)
                    {
                        Projectile.Kill();
                    }
                    break;
                case 2:
                    if (Projectile.ai[0] >= explosionTime)
                    {
                        Projectile.Kill();
                    }
                    break;
            }

            Projectile.ai[0] += 1f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, System.Int32 damageDone)
        {
            if (state == 0)
            {
                ExplodeBig();
                AdvanceState();
            }
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
    }
}