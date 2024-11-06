using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.BrushProjectiles
{
    internal class InkbrushProjectile : BaseProjectile
    {
        private Color bulletColor;
        private float delayUntilFall;
        private float shotGravity = 0.05f;
        private readonly float airResist = 0.995f;
        private float drawScale = 0;
        private float drawRotation;
        protected float brightness = 0.001f;

        protected float damageFallOffMod = 0.75f;

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
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseBrush;

            shotGravity = weaponData.ShotGravity;
            delayUntilFall = weaponData.DelayUntilFall;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();

            // Set visuals
            Projectile.frame = Main.rand.Next(0, Main.projFrames[Projectile.type]);
            bulletColor = GenerateInkColor();
            drawRotation += MathHelper.ToRadians(Main.rand.Next(0, 359));
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.position, bulletColor.R * brightness, bulletColor.G * brightness, bulletColor.B * brightness);

            if (timeSpentAlive > 3 * FrameSpeed())
            {
                if (drawScale < 1f)
                {
                    drawScale += 0.1f;
                }

                if (Timer % 8 == 0 && Main.rand.NextBool(10))
                {
                    Dust.NewDustPerfect(
                        Position: Projectile.Center + Main.rand.NextVector2Circular(5, 5),
                        Type: ModContent.DustType<SplatterDropletDust>(),
                        Velocity: Projectile.velocity * 0.2f,
                        newColor: GenerateInkColor(),
                        Scale: Main.rand.NextFloat(1f, 1.5f)
                    );
                }
            }

            // Rotation increased by velocity.X 
            drawRotation += Math.Sign(Projectile.velocity.X) * 0.1f;
            if (Math.Abs(Projectile.velocity.X) > 0)
            {
                Projectile.velocity.X *= airResist;
            }

            Timer++;

            if (timeSpentAlive >= delayUntilFall * FrameSpeed())
            {
                Projectile.velocity.Y += shotGravity;
            }

            if (timeSpentAlive >= delayUntilFall / 2 * FrameSpeed() && damageFallOffMod > 0.5f)
            {
                damageFallOffMod -= 0.01f;
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
            if (timeSpentAlive < 5) return false;

            DrawProjectile(inkColor: bulletColor, rotation: drawRotation, scale: drawScale, alphaMod: 0.5f, considerWorldLight: false, additiveAmount: 1f);
            return false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            var size = 30;
            hitbox = new Rectangle((int)Projectile.Center.X - size / 2, (int)Projectile.Center.Y - size / 2, size, size);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.SourceDamage *= damageFallOffMod;
        }
    }
}
