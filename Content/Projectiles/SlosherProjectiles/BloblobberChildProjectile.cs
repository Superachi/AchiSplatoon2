using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SlosherProjectiles
{
    internal class BloblobberChildProjectile : BaseProjectile
    {
        private int delayUntilFall;
        private float fallSpeed;
        private bool canFall = false;
        private bool countedForBurst = false;
        private Color bulletColor;

        private int bounceCount = 0;
        private float drawScale = 0f;
        private float drawScaleGoal = 1f;
        private float drawAlpha = 0f;
        private float previousVelocityX;
        private float previousVelocityY;

        private const int stateSpawn = 0;
        private const int stateFly = 1;
        private const int stateDespawn = 2;

        protected float Timer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 16;
            Projectile.timeLeft = 300 * FrameSpeed();
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseSlosher;

            delayUntilFall = 5;
            shootSample = weaponData.ShootSample;
            fallSpeed = weaponData.ShotGravity / FrameSpeed();
        }

        public override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
            drawScaleGoal += Main.rand.NextFloat(-0.2f, 0.2f);
            bulletColor = GenerateInkColor();
        }

        public override bool PreAI()
        {
            base.PreAI();
            previousVelocityX = Projectile.velocity.X;
            previousVelocityY = Projectile.velocity.Y;

            return true;
        }

        public override void AI()
        {
            switch (state)
            {
                case stateSpawn:
                    if (drawAlpha < 0.8f) drawAlpha += 0.2f / FrameSpeed();
                    if (drawScale < drawScaleGoal) drawScale += 0.2f / FrameSpeed();
                    if (timeSpentAlive > 30 * FrameSpeed()) AdvanceState();
                    break;

                case stateFly:
                    if (timeSpentAlive > 180 * FrameSpeed() || bounceCount >= 5) AdvanceState();
                    break;

                case stateDespawn:
                    Projectile.friendly = false;
                    drawAlpha *= 0.99f;
                    drawScale *= 0.99f;

                    if (drawScale < 0.01f || drawAlpha < 0.01f)
                    {
                        Projectile.Kill();
                        return;
                    }
                    break;
            }

            Timer ++;
            if (Timer >= delayUntilFall * FrameSpeed())
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

            if (Main.rand.NextBool(200) && state < stateDespawn)
            {
                Dust.NewDustPerfect(
                    Position: Projectile.Center + Main.rand.NextVector2Circular(20f * drawScale, 20f * drawScale),
                    Type: ModContent.DustType<SplatterDropletDust>(),
                    Velocity: Projectile.velocity * 2,
                    newColor: GenerateInkColor(),
                    Scale: Main.rand.NextFloat(1f, 1.5f));
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (!countedForBurst)
            {
                countedForBurst = true;
                var parentProj = GetParentProjectile(parentIdentity);

                if (parentProj.ModProjectile is BloblobberMainProjectile)
                {
                    var parentModProj = parentProj.ModProjectile as BloblobberMainProjectile;
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
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bool hasBounced = false;
            bool horizontalBounce = false;
            if (Projectile.velocity.X != previousVelocityX)
            {
                hasBounced = true;
                horizontalBounce = true;
                Projectile.position.X -= previousVelocityX;
                Projectile.velocity.X = -previousVelocityX;
            }

            if (Projectile.velocity.Y != previousVelocityY)
            {
                hasBounced = true;
                Projectile.position.Y -= previousVelocityY;
                Projectile.velocity.Y = -previousVelocityY;
                Projectile.velocity.Y *= 0.8f;

                if (horizontalBounce) Projectile.velocity.Y *= -1f;
            }

            if (hasBounced) bounceCount++;

            Projectile.velocity *= 0.8f;
            drawScale *= 0.8f;

            Timer = 3;
            canFall = false;
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawProjectile(inkColor: bulletColor, rotation: 0, scale: drawScale, alphaMod: drawAlpha, considerWorldLight: false);
            return false;
        }

        // Netcode
        protected override void NetSendSyncMovement(BinaryWriter writer)
        {
            writer.Write((bool)canFall);
        }

        protected override void NetReceiveSyncMovement(BinaryReader reader)
        {
            canFall = reader.ReadBoolean();
        }
    }
}
