using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Projectiles.ProjectileVisuals;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.BrushProjectiles
{
    internal class DesertBrushProjectile : BaseProjectile
    {
        private Color bulletColor;
        private float airResist;
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
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 2;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 360;
            Projectile.tileCollide = true;
            Projectile.penetrate = 3;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseBrush;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();

            airResist = 0.97f;
            enablePierceDamagefalloff = false;

            // Audio
            SoundHelper.PlayAudio(SoundID.Item45, 0.3f, maxInstances: 10, pitch: 0.8f, pitchVariance: 0.2f, position: Projectile.Center);
            SoundHelper.PlayAudio(SoundID.Item66, 0.2f, maxInstances: 10, pitch: 0.3f, pitchVariance: 0.2f, position: Projectile.Center);

            // Set visuals
            drawRotation += MathHelper.ToRadians(Main.rand.Next(0, 359));
            drawScale += Main.rand.NextFloat(1f, 1.5f);
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.position, bulletColor.R * brightness, bulletColor.G * brightness, bulletColor.B * brightness);

            // Rotation increased by velocity.X 
            var rotateDirection = Math.Sign(Projectile.velocity.X);
            var rotateSpeed = Math.Abs(Projectile.velocity.Length()) * 0.05f + 0.05f;
            drawRotation += rotateDirection * rotateSpeed;
            Projectile.velocity *= airResist;

            if (Projectile.timeLeft < 40 && drawScale > 0)
            {
                drawScale -= 0.05f;
            }
            else if (drawScale < 2f && timeSpentAlive > 2 * FrameSpeed())
            {
                drawScale += 0.1f;
            }

            if (Projectile.timeLeft == 1)
            {
                var sparkle = CreateChildProjectile<StillSparkleVisual>(Projectile.Center, Vector2.Zero, 0, true);
                sparkle.UpdateCurrentColor(CurrentColor);
                sparkle.AdjustRotation(0);
            }

            // Spawn dust
            Timer++;
            if (Timer % 3 == 0 && Main.rand.NextBool(2))
            {
                var d = Dust.NewDustPerfect(
                    Position: Projectile.position + Main.rand.NextVector2CircularEdge(22, 22),
                    Type: DustID.PortalBolt,
                    Velocity: Projectile.velocity * 0.5f,
                    newColor: GenerateInkColor(),
                    Scale: Main.rand.NextFloat(0.8f, 1.2f)
                );

                d.noGravity = true;
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var sparkle = CreateChildProjectile<StillSparkleVisual>(Projectile.Center + Projectile.velocity, Vector2.Zero, 0, true);
            sparkle.UpdateCurrentColor(CurrentColor);
            sparkle.AdjustRotation(0);

            for (int i = 0; i < 5; i ++)
            {
                var d = Dust.NewDustPerfect(
                    Position: target.Center,
                    Type: DustID.PortalBolt,
                    Velocity: Main.rand.NextVector2CircularEdge(5, 5),
                    newColor: GenerateInkColor(),
                    Scale: Main.rand.NextFloat(0.8f, 1.2f)
                );

                d.noGravity = true;
            }

            base.OnHitNPC(target, hit, damageDone);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            SetHitboxSize(30, out hitbox);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (timeSpentAlive < 5 * FrameSpeed()) return false;

            var color = ColorHelper.LerpBetweenColorsPerfect(CurrentColor, Color.White, 0.8f);

            for (int i = 0; i < 4; i ++)
            {
                var rotation = drawRotation + i * 45;

                DrawProjectile(
                    inkColor: color,
                    rotation: rotation,
                    scale: drawScale * (1 - i * 0.1f),
                    alphaMod: 0.2f + i * 0.1f,
                    considerWorldLight: false,
                    additiveAmount: 1f);
            }

            return false;
        }
    }
}
