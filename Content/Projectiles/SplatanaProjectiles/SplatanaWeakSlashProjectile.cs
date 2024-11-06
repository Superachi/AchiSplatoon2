using AchiSplatoon2.Content.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles
{
    internal class SplatanaWeakSlashProjectile : BaseProjectile
    {
        protected virtual bool Animate => true;
        protected virtual bool ProjectileDust => true;

        protected Color bulletColor;
        protected virtual int FrameCount => 4;
        protected virtual int FrameDelay => 12;
        protected float frameTimer = 0;
        protected float drawScale = 1f;

        protected int timeLeftWhenFade = 20;
        protected bool fading = false;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = FrameCount;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 2;
        }

        public override void AfterSpawn()
        {
            Initialize();
            bulletColor = GenerateInkColor();
            if (Animate) Projectile.frame = Main.rand.Next(FrameCount);
        }

        public override void AI()
        {
            if (Animate)
            {
                frameTimer += FrameSpeedDivide(1);
                if (frameTimer >= FrameDelay)
                {
                    frameTimer = 0;
                    Projectile.frame = (Projectile.frame + 1) % FrameCount;
                }
            }

            if (Projectile.timeLeft <= timeLeftWhenFade && !fading)
            {
                fading = true;
            }

            if (Main.rand.NextBool(5) && !fading && ProjectileDust)
            {
                Color dustColor = GenerateInkColor();
                Dust.NewDustPerfect(
                    Position: Projectile.Center + Main.rand.NextVector2Circular(Projectile.width / 2, Projectile.height / 2),
                    Type: ModContent.DustType<SplatterBulletDust>(),
                    Velocity: Projectile.velocity / Main.rand.Next(4, 8),
                    newColor: dustColor,
                    Scale: Main.rand.NextFloat(1f, 1.5f));

                Dust.NewDustPerfect(
                    Position: Projectile.Center + Main.rand.NextVector2Circular(Projectile.width / 2, Projectile.height / 2),
                    Type: ModContent.DustType<SplatterDropletDust>(),
                    Velocity: Projectile.velocity / Main.rand.Next(4, 8),
                    newColor: dustColor,
                    Scale: Main.rand.NextFloat(1f, 1.5f));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (timeSpentAlive > 5)
            {
                var rotation = Projectile.velocity.ToRotation();

                float alpha = 1f;
                if (fading)
                {
                    alpha = (float)Projectile.timeLeft / (float)timeLeftWhenFade;
                }
                float scale = 1f + (float)Math.Sin(MathHelper.ToRadians(timeSpentAlive * 8)) * 0.1f;
                DrawProjectile(bulletColor, rotation, scale: scale, alphaMod: alpha, considerWorldLight: false);
            }

            return false;
        }
    }
}
