using AchiSplatoon2.Content.Dusts;
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
            Projectile.timeLeft = ExtraUpdatesTime(120);
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
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
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.alpha = 255;
                Projectile.Resize(finalExplosionRadius, finalExplosionRadius);
                explosionDustModel = new ExplosionDustModel(_dustMaxVelocity: 15f, _dustAmount: 20, _minScale: 1, _maxScale: 2, _radiusModifier: finalExplosionRadius);
                EmitBurstDust(explosionDustModel);
                PlayAudio("BlasterExplosionLight", volume: 0.1f, pitchVariance: 0.2f, maxInstances: 10);
            }
            NetUpdate(ProjNetUpdateType.DustExplosion);
        }

        public override void AI()
        {
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

                Lighting.AddLight(Projectile.Center, GenerateInkColor().ToVector3());

                if (Projectile.timeLeft < ExtraUpdatesTime(3) && !hasExploded)
                {
                    Explode();
                    NetUpdate(ProjNetUpdateType.DustExplosion);
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
                    Projectile.timeLeft = ExtraUpdatesTime(60 + (int)Projectile.ai[2] * 5);
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
                Explode();
                NetUpdate(ProjNetUpdateType.DustExplosion);
            }
        }

        protected override void NetSendDustExplosion(BinaryWriter writer)
        {
            // Dust information
            writer.Write((double)explosionDustModel.dustMaxVelocity);
            writer.Write((Int16)explosionDustModel.dustAmount);
            writer.Write((double)explosionDustModel.minScale);
            writer.Write((double)explosionDustModel.maxScale);
            writer.Write((Int16)explosionDustModel.radiusModifier);

            // Size/position information
            writer.Write((Int16)Projectile.width);
            writer.Write((Int16)Projectile.height);
            writer.WriteVector2(Projectile.velocity);
            writer.WriteVector2(Projectile.position);
        }

        protected override void NetReceiveDustExplosion(BinaryReader reader)
        {
            // Override in child class to spawn dusts, play sound, etc.
            explosionDustModel = new ExplosionDustModel(
                (float)reader.ReadDouble(),
                (int)reader.ReadInt16(),
                (float)reader.ReadDouble(),
                (float)reader.ReadDouble(),
                (int)reader.ReadInt16()
            );

            var w = (int)reader.ReadInt16();
            var h = (int)reader.ReadInt16();
            var vel = (Vector2)reader.ReadVector2();
            var pos = (Vector2)reader.ReadVector2();

            Projectile.Resize(w, h);
            Projectile.velocity = vel;
            Projectile.position = pos;

            Projectile.alpha = 255;
            EmitBurstDust(explosionDustModel);
            PlayAudio("BlasterExplosionLight", volume: 0.1f, pitchVariance: 0.2f, maxInstances: 10);
        }
    }
}