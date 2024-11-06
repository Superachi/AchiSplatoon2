using AchiSplatoon2.Content.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.RollerProjectiles
{
    internal class RollerInkProjectile : BaseProjectile
    {
        private Color bulletColor;
        public float delayUntilFall = 45;
        private readonly float fallSpeed = 0.4f;
        private float drawScale;
        private float drawRotation;
        protected float brightness = 0.001f;
        private bool visible;
        private float damageFalloffMod = 1f;

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

        protected override void AfterSpawn()
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

            if (timeSpentAlive > 10)
            {
                if (damageFalloffMod > 0.5f)
                {
                    damageFalloffMod -= 0.005f;
                }
            }

            // Rotation increased by velocity.X 
            drawRotation += Math.Sign(Projectile.velocity.X) * 0.1f;

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

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.FinalDamage *= damageFalloffMod;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            target.immune[Projectile.owner] = 12;
        }
    }
}
