using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Projectiles.ProjectileVisuals;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.BrushProjectiles
{
    internal class DesertBrushProjectile : BaseProjectile
    {
        protected override float DamageModifierAfterPierce => 0.95f;

        private float airResist;
        private float drawScale;
        private float drawRotation;
        protected float brightness = 0.001f;

        private float drawAlpha;

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
            Projectile.timeLeft = 120;
            Projectile.tileCollide = true;
            Projectile.penetrate = 3;

            ProjectileID.Sets.TrailCacheLength[Type] = 14;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseBrush;
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            ApplyWeaponInstanceData();

            airResist = 0.96f;
            enablePierceDamagefalloff = false;

            // Audio
            SoundHelper.PlayAudio(SoundID.DD2_SonicBoomBladeSlash, 0.2f, maxInstances: 10, pitch: 1f, pitchVariance: 0.2f, position: Projectile.Center);
            SoundHelper.PlayAudio(SoundID.Item169, 0.9f, maxInstances: 10, pitch: 0.5f, pitchVariance: 0.2f, position: Projectile.Center);
            PlayAudio(SoundID.DD2_DarkMageHealImpact, volume: 0.1f, pitchVariance: 0.3f, pitch: 1f, maxInstances: 10);

            // Set visuals
            UpdateCurrentColor(ColorHelper.AddRandomHue(20, CurrentColor));
            drawRotation += MathHelper.ToRadians(Main.rand.Next(0, 359));
            drawScale += Main.rand.NextFloat(1f, 1.5f);
            drawAlpha = 0;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.position, CurrentColor.R * brightness, CurrentColor.G * brightness, CurrentColor.B * brightness);

            // Rotation increased by velocity.X 
            var rotateDirection = Math.Sign(Projectile.velocity.X);
            var rotateSpeed = 0.2f;
            drawRotation += rotateDirection * rotateSpeed;
            Projectile.velocity *= airResist;

            if (Projectile.timeLeft < 60 && drawScale > 0)
            {
                drawScale -= 0.05f;
                drawAlpha -= 0.01f;
                if (drawScale <= 0.05f)
                {
                    Projectile.Kill();
                    return;
                }
            }
            else 
            {
                if (drawScale < 2f && timeSpentAlive > 2 * FrameSpeed())
                {
                    drawScale += 0.1f;
                }

                if (drawAlpha < 1f)
                {
                    drawAlpha += 0.003f;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ProjectileBounce(oldVelocity, new Vector2(0.5f, 0.5f));
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var sparkle = CreateChildProjectile<StillSparkleVisual>(target.Center + Projectile.velocity, Vector2.Zero, 0, true);
            sparkle.UpdateCurrentColor(CurrentColor);
            sparkle.AdjustRotation(MathHelper.ToRadians(45));

            for (int i = 0; i < 6; i ++)
            {
                var direction = i % 2 == 0 ? -1 : 1;

                var d = Dust.NewDustPerfect(
                    Position: target.Center,
                    Type: DustID.WhiteTorch,
                    Velocity: Vector2.Normalize(Projectile.velocity).RotatedBy(MathHelper.ToRadians(90)) * (i + 1) * direction + Main.rand.NextVector2Circular(3, 3),
                    newColor: GenerateInkColor(),
                    Scale: Main.rand.NextFloat(2f, 3f)
                );

                d.color = CurrentColor;
                d.noGravity = true;
                d.noLight = true;
            }

            base.OnHitNPC(target, hit, damageDone);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            SetHitboxSize(30, out hitbox);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < 6; i++)
            {
                var rotation = drawRotation + i * 0.3f;

                DrawProjectile(
                    inkColor: ColorHelper.LerpBetweenColorsPerfect(CurrentColor, Color.White, i * 0.15f),
                    rotation: rotation,
                    scale: drawScale,
                    alphaMod: drawAlpha * (i * 1f),
                    considerWorldLight: false,
                    additiveAmount: 1f);
            }

            return false;
        }
    }
}
