using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles.EelSplatana
{
    internal class EelSplatanaStrongSlashProjectile : SplatanaStrongSlashProjectile
    {
        protected override bool ProjectileDust => false;
        protected override int FrameCount => 8;
        protected override int FrameDelay => 3;

        private float drawAlpha;
        private bool movementStuck = false;
        private Vector2 directionVector;

        private float previousPositionX;
        private float previousPositionY;
        private readonly int shootSpeed = 9;
        private int shootCooldown = 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 24;
            Projectile.height = 24;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = FrameCount;
        }

        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            enablePierceDamagefalloff = false;
            dissolvable = false;
            Projectile.localNPCHitCooldown = 5 * FrameSpeed();
            Projectile.penetrate = -1;
            Projectile.damage /= 2;

            DespawnOtherTornados();
            directionVector = Vector2.Normalize(Projectile.velocity);
            Projectile.frame = Main.rand.Next(FrameCount);
            drawAlpha = 0f;
            drawScale = 0.5f;
        }

        private void DespawnOtherTornados()
        {
            foreach (var projectile in Main.ActiveProjectiles)
            {
                if (projectile.ModProjectile is EelSplatanaStrongSlashProjectile
                    && projectile.identity != Projectile.identity
                    && projectile.owner == Projectile.owner)
                {
                    projectile.timeLeft = timeLeftWhenFade;
                }
            }
        }

        public override void AI()
        {
            base.AI();


            float animationTime = 40f;
            if (timeSpentAlive < (int)animationTime)
            {
                if (drawAlpha < 0.8f) drawAlpha += 1f / animationTime;
                if (drawScale < 2) drawScale += 1f / animationTime;
            }

            if (Projectile.velocity.Length() > 1)
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, directionVector, 0.03f);
            }

            // Detect enemies within range and shoot projectiles at them
            if (IsThisClientTheProjectileOwner())
            {
                if (shootCooldown > 0) shootCooldown--;
                else
                {
                    shootCooldown = shootSpeed * FrameSpeed();
                    NPC target = FindClosestEnemy(500);
                    if (target != null && CanHitNPCWithLineOfSight(target))
                    {
                        var shotPosition = Projectile.Center + Main.rand.NextVector2Circular(32, 32);
                        var targetPosition = target.Center + target.velocity;
                        var shotSpeed = 4f;
                        var targetDir = shotPosition.DirectionTo(targetPosition);
                        CreateChildProjectile<EelSplatanaSmallProjectile>(shotPosition, targetDir * shotSpeed, Projectile.damage * 2);
                    }
                }
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            var size = 64;
            hitbox = new Rectangle((int)Projectile.Center.X - size / 2, (int)Projectile.Center.Y - size / 2, size, size);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (timeSpentAlive > 5)
            {
                if (fading)
                {
                    drawAlpha = Projectile.timeLeft / (float)timeLeftWhenFade;
                }
                float scale = drawScale + (float)Math.Sin(MathHelper.ToRadians(timeSpentAlive * 2)) * 0.05f;
                DrawProjectile(Color.White, rotation: 0, scale: scale, alphaMod: drawAlpha, considerWorldLight: false, positionOffset: new Vector2(0, -32));
            }

            return false;
        }
        public override bool PreAI()
        {
            base.PreAI();
            previousPositionX = Projectile.position.X;
            previousPositionY = Projectile.position.Y;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Math.Abs(Projectile.position.X) - Math.Abs(previousPositionX) < 0.05f
                && Math.Abs(Projectile.position.Y) - Math.Abs(previousPositionY) < 0.05f
                && !movementStuck)
            {
                movementStuck = true;
                Projectile.timeLeft /= 2;
                Projectile.timeLeft = Math.Max(Projectile.timeLeft, timeLeftWhenFade);
            }
            return false;
        }
    }
}
