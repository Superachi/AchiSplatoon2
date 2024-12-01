using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles.EelSplatana
{
    internal class EelSplatanaSmallProjectile : BaseProjectile
    {
        private readonly int delayUntilFall = 60;
        private readonly float fallSpeed = 0.1f;
        private bool canFall = false;
        private readonly float terminalVelocity = 6f;
        protected float Timer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;

            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 5;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            Projectile.penetrate = 2;
            enablePierceDamagefalloff = true;
            wormDamageReduction = true;
            PlayAudio("BrushShootAlt", volume: 0.1f, pitchVariance: 0.3f, pitch: 0.5f, maxInstances: 3);
        }

        public override void AI()
        {
            Timer++;
            if (Timer >= FrameSpeed(delayUntilFall))
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

            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.spriteDirection = Projectile.velocity.X < 0 ? -1 : 1;
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

        public override bool PreDraw(ref Color lightColor)
        {
            var len = Projectile.oldPos.Length;
            for (int i = 0; i < len; i++)
            {
                if (i % 5 != 0) continue;

                var posDiff = Projectile.position - Projectile.oldPos[i];
                var spriteEffects = Projectile.oldSpriteDirection[i] == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None;
                var alphaMod = (float)i / len;

                DrawProjectile(Color.White, Projectile.oldRot[i], scale: 1, alphaMod: alphaMod, considerWorldLight: true, flipSpriteSettings: spriteEffects, positionOffset: posDiff);
            }
            return false;
        }
    }
}
