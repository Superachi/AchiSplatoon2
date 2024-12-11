using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles
{
    internal class SplatanaWeakSlashProjectile : BaseProjectile
    {
        protected virtual bool ProjectileDust => true;

        protected Color bulletColor;
        protected virtual int FrameCount => 4;
        protected virtual int FrameDelay => 12;
        protected float frameTimer = 0;
        protected float drawScale = 1f;

        protected int timeLeftWhenFade = 20;
        protected bool fading = false;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 2;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            bulletColor = CurrentColor;
        }

        public override void AI()
        {
            if (Projectile.timeLeft <= timeLeftWhenFade && !fading)
            {
                fading = true;
            }

            if (ProjectileDust && timeSpentAlive > 16)
            {
                Color dustColor = CurrentColor;

                if (timeSpentAlive % 4 == 0 && !fading)
                {
                    DustHelper.NewChargerBulletDust(
                        position: Projectile.Center + Main.rand.NextVector2Circular(10, 10),
                        velocity: Projectile.velocity / Main.rand.Next(2, 4),
                        color: dustColor,
                        minScale: 1f,
                        maxScale: 1.5f);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (timeSpentAlive > 8)
            {
                var rotation = Projectile.velocity.ToRotation();

                float alpha = 1f;
                if (fading)
                {
                    alpha = (float)Projectile.timeLeft / (float)timeLeftWhenFade;
                }
                float scale = 1f + (float)Math.Sin(MathHelper.ToRadians(timeSpentAlive * 8)) * 0.1f;
                DrawProjectile(ColorHelper.ColorWithAlpha255(bulletColor), rotation, scale: scale, alphaMod: alpha * 0.6f, considerWorldLight: false, additiveAmount: 1f);
            }

            return false;
        }
    }
}
