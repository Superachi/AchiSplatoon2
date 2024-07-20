using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles
{
    internal class SplatanaWeakSlashProjectile : BaseProjectile
    {
        protected virtual bool Animate => true;

        private Color bulletColor;
        private int frameCount = 4;
        private float frameTimer = 0;
        private int frameDelay = 16;
        private int currentFrame = 0;
        private float drawScale = 1f;

        private int timeLeftWhenFade = 20;
        private bool fading = false;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = frameCount;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = 3;
        }

        public override void AfterSpawn()
        {
            Initialize();
            bulletColor = GenerateInkColor();
            if (Animate) Projectile.frame = Main.rand.Next(frameCount);
        }

        public override void AI()
        {
            if (Animate)
            {
                frameTimer += FrameSpeedDivide(1);
                if (frameTimer >= frameDelay)
                {
                    frameTimer = 0;
                    Projectile.frame = (Projectile.frame + 1) % frameCount;
                }
            }

            if (Projectile.timeLeft <= timeLeftWhenFade && !fading)
            {
                fading = true;
            }

            if (Main.rand.NextBool(5) && !fading)
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
                    Main.NewText($"{Projectile.timeLeft} / {timeLeftWhenFade} = {alpha}");
                }
                float scale = 1f + (float)Math.Sin(MathHelper.ToRadians(timeSpentAlive * 8)) * 0.1f;
                DrawProjectile(bulletColor, rotation, scale: scale, alphaMod: alpha, considerWorldLight: false);
            }
            
            return false;
        }
    }
}
