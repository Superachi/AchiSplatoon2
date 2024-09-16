using AchiSplatoon2.Content.Dusts;
using System.IO;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles.EelSplatana
{
    internal class EelSplatanaSmallProjectile : BaseProjectile
    {
        private int delayUntilFall = 60;
        private float fallSpeed = 0.1f;
        private bool canFall = false;
        private float terminalVelocity = 6f;
        protected float Timer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 5;
        }

        public override void AfterSpawn()
        {
            Initialize();
            Projectile.penetrate = 2;
            enablePierceDamagefalloff = true;
            wormDamageReduction = true;
            PlayAudio("BrushShootAlt", volume: 0.1f, pitchVariance: 0.3f, pitch: 1f, maxInstances: 3);
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
            var rotation = Projectile.velocity.ToRotation();
            SpriteEffects faceDir = SpriteEffects.None;
            if (Projectile.velocity.X < 0) faceDir = SpriteEffects.FlipVertically;

            DrawProjectile(Color.White, rotation, scale: 1, alphaMod: timeSpentAlive / 14, considerWorldLight: true, flipSpriteSettings: faceDir);
            return false;
        }
    }
}
