using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.BrushProjectiles
{
    internal class DesertBrushProjectile : BaseProjectile
    {
        private Color bulletColor;
        private readonly float airResist = 0.98f;
        private float drawScale;
        private float drawRotation;
        protected float brightness = 0.001f;

        protected float Timer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 2;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseBrush;
        }

        public override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();

            // Set visuals
            Projectile.frame = Main.rand.Next(0, Main.projFrames[Projectile.type]);
            bulletColor = GenerateInkColor();
            drawRotation += MathHelper.ToRadians(Main.rand.Next(0, 359));
            drawScale += Main.rand.NextFloat(1f, 1.5f);
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.position, bulletColor.R * brightness, bulletColor.G * brightness, bulletColor.B * brightness);

            // Rotation increased by velocity.X 
            drawRotation += Math.Sign(Projectile.velocity.X) * 0.1f;
            Projectile.velocity *= airResist;

            if (Projectile.timeLeft < 18)
            {
                drawScale -= 0.05f;
            } else if (drawScale < 1f && timeSpentAlive > 2 * FrameSpeed())
            {
                drawScale += 0.1f;
            }

            // Spawn dust
            Timer++;
            if (Timer % 5 == 0 && Main.rand.NextBool(10) && IsVelocityGreaterThan(0.5f))
            {
                Dust.NewDustPerfect(
                    Position: Projectile.position + Main.rand.NextVector2Circular(10, 10),
                    Type: ModContent.DustType<SplatterBulletLastingDust>(),
                    Velocity: Projectile.velocity * 0.2f,
                    newColor: GenerateInkColor(),
                    Scale: Main.rand.NextFloat(1f, 1.5f)
                );
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ProjectileBounce(oldVelocity, new Vector2(0.8f, 0.8f));
            for (int i = 0; i < 5; i++)
            {
                float random = Main.rand.NextFloat(-2, 2);
                float velX = ((Projectile.velocity.X + random) * -0.5f);
                float velY = ((Projectile.velocity.Y + random) * -0.5f);
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: GenerateInkColor(), Scale: Main.rand.NextFloat(0.8f, 1.6f));
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (timeSpentAlive < 5 * FrameSpeed()) return false;

            DrawProjectile(inkColor: bulletColor, rotation: drawRotation, scale: drawScale, alphaMod: 0.5f, considerWorldLight: false, additiveAmount: 1f);
            return false;
        }
    }
}
