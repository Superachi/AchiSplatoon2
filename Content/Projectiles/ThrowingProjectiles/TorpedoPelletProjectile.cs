using Microsoft.Xna.Framework;
using System.IO;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class TorpedoPelletProjectile : BaseProjectile
    {
        private readonly int delayUntilFall = 10;
        private readonly float fallSpeed = 0.2f;
        private bool canFall = false;
        private bool hasExploded = false;

        public int explosionRadius;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = -1;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();

            enablePierceDamagefalloff = false;
            wormDamageReduction = true;
            Projectile.velocity.X *= 0.7f;
        }

        public override void AI()
        {
            if (timeSpentAlive > 30)
            {
                Projectile.friendly = true;
            }

            if (timeSpentAlive >= FrameSpeed(delayUntilFall))
            {
                if (!canFall)
                {
                    canFall = true;
                    NetUpdate(ProjNetUpdateType.SyncMovement, true);
                }
            }

            if (canFall)
            {
                Projectile.velocity.Y += FrameSpeedDivide(fallSpeed);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!hasExploded)
            {
                hasExploded = true;

                Projectile.timeLeft = 6;
                Projectile.tileCollide = false;

                Projectile.position -= Projectile.velocity;
                Projectile.velocity = Vector2.Zero;
                Projectile.Resize(explosionRadius, explosionRadius);

                PlayAudio("BlasterExplosion", volume: 0.1f, pitchVariance: 0.3f, maxInstances: 20, pitch: 0.5f, position: Projectile.Center);
                TripleHitDustBurst(Projectile.Center, false);
            }

            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (hasExploded) return false;

            DrawProjectile(inkColor: CurrentColor, rotation: 0, considerWorldLight: false, additiveAmount: 1f);
            return false;
        }

        // Netcode
        protected override void NetSendSyncMovement(BinaryWriter writer)
        {
            writer.Write(canFall);
        }

        protected override void NetReceiveSyncMovement(BinaryReader reader)
        {
            canFall = reader.ReadBoolean();
        }
    }
}
