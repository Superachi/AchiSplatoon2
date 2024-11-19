using AchiSplatoon2.Content.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

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
            bulletColor = GenerateInkColor();
        }

        public override void AI()
        {
            if (Projectile.timeLeft <= timeLeftWhenFade && !fading)
            {
                fading = true;
            }

            if (ProjectileDust && timeSpentAlive > 16)
            {
                Color dustColor = GenerateInkColor();

                if (timeSpentAlive % 4 == 0)
                {
                    Dust.NewDustPerfect(
                        Position: Projectile.Center + Main.rand.NextVector2Circular(10, 10),
                        Type: ModContent.DustType<ChargerBulletDust>(),
                        Velocity: -Projectile.velocity / Main.rand.Next(2, 4),
                        newColor: dustColor,
                        Scale: Main.rand.NextFloat(1f, 1.5f));
                }
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
