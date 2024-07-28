using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Rollers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.RollerProjectiles
{
    internal class RollerInkProjectile : BaseProjectile
    {
        private Color bulletColor;
        public float delayUntilFall = 20;
        private float fallSpeed = 0.3f;
        private float airResist = 0.99f;
        private float drawScale;
        private float drawRotation;
        protected float brightness = 0.001f;
        private bool visible;

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
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }

        public override void AfterSpawn()
        {
            Initialize();

            // Set visuals
            Projectile.frame = Main.rand.Next(0, Main.projFrames[Projectile.type]);
            bulletColor = GenerateInkColor();
            drawRotation += MathHelper.ToRadians(Main.rand.Next(0, 359));
            drawScale += Main.rand.NextFloat(0.8f, 1.6f);
        }

        public override void AI()
        {
            var owner = GetOwner();
            if (!visible && Vector2.Distance(owner.Center, Projectile.Center) > 30)
            {
                visible = true;
            }


            // Rotation increased by velocity.X 
            drawRotation += Math.Sign(Projectile.velocity.X) * 0.1f;
            if (Math.Abs(Projectile.velocity.X) > 0)
            {
                Projectile.velocity.X *= airResist;
            }

            Timer++;

            // Start falling eventually
            if (Projectile.ai[0] >= delayUntilFall * FrameSpeed())
            {
                Projectile.velocity.Y += fallSpeed;
            }

            // Spawn dust
            if (visible && Main.rand.NextBool(4))
            {
                Lighting.AddLight(Projectile.position, bulletColor.R * brightness, bulletColor.G * brightness, bulletColor.B * brightness);
                Dust.NewDust(
                    Position: Projectile.position,
                    Width: Projectile.width,
                    Height: Projectile.height,
                    Type: ModContent.DustType<SplatterDropletDust>(),
                    newColor: GenerateInkColor(),
                    Scale: Main.rand.NextFloat(1f, 1.5f)
                );
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 5; i++)
            {
                float random = Main.rand.NextFloat(-2, 2);
                float velX = ((Projectile.velocity.X + random) * -0.5f);
                float velY = ((Projectile.velocity.Y + random) * -0.5f);
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: GenerateInkColor(), Scale: Main.rand.NextFloat(0.8f, 1.6f));
            }
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (visible)
            {
                DrawProjectile(inkColor: bulletColor, rotation: drawRotation, scale: drawScale, considerWorldLight: false);
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 9;
        }
    }
}
