using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles.EelSplatana
{
    internal class EelSplatanaStrongSlashProjectile : SplatanaStrongSlashProjectile
    {
        protected override bool ProjectileDust => false;
        protected override int FrameCount => 6;
        protected override int FrameDelay => 2;

        private float drawAlpha;
        private bool movementStuck = false;
        private Vector2 directionVector;

        private float previousPositionX;
        private float previousPositionY;
        private readonly int shootSpeed = 9;
        private int shootCooldown = 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 20;
            Projectile.height = 20;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = FrameCount;
        }

        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            enablePierceDamagefalloff = false;
            dissolvable = false;
            Projectile.localNPCHitCooldown = 5 * FrameSpeed();
            Projectile.penetrate = -1;
            Projectile.damage /= 2;

            DespawnOtherTornados();
            directionVector = Vector2.Normalize(Projectile.velocity);
            Projectile.frame = Main.rand.Next(FrameCount);
            drawAlpha = 0f;
            drawScale = 0.5f;

            PlayAudio(SoundID.DD2_WyvernDiveDown, volume: 0.8f, pitchVariance: 0.3f, pitch: 0.5f, maxInstances: 10);
            PlayAudio(SoundID.Item117, volume: 0.2f, pitchVariance: 0.3f, pitch: -0.5f, maxInstances: 10);
            PlayAudio(SoundID.Item131, volume: 0.2f, pitchVariance: 0.3f, pitch: 0f, maxInstances: 10);
            PlayAudio(SoundID.Drown, volume: 0.3f, pitchVariance: 0.3f, pitch: 0.5f, maxInstances: 10);
        }

        private void DespawnOtherTornados()
        {
            foreach (var projectile in Main.ActiveProjectiles)
            {
                if (projectile.ModProjectile is EelSplatanaStrongSlashProjectile
                    && projectile.identity != Projectile.identity
                    && projectile.owner == Projectile.owner)
                {
                    projectile.timeLeft = timeLeftWhenFade;
                }
            }
        }

        public override void AI()
        {
            base.AI();

            frameTimer += FrameSpeedDivide(1);
            if (frameTimer >= FrameDelay)
            {
                frameTimer = 0;
                Projectile.frame = (Projectile.frame + 1) % FrameCount;
            }

            float animationTime = 40f;
            if (timeSpentAlive < (int)animationTime)
            {
                if (drawAlpha < 0.8f) drawAlpha += 1f / animationTime;
                if (drawScale < 2) drawScale += 1f / animationTime;
            }

            Projectile.velocity += Projectile.Center.DirectionTo(Main.MouseWorld) / 60f;
            if (Projectile.velocity.Length() > 2)
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, directionVector, 0.03f);
            }

            // Detect enemies within range and shoot projectiles at them
            if (IsThisClientTheProjectileOwner())
            {
                if (shootCooldown > 0) shootCooldown--;
                else
                {
                    shootCooldown = shootSpeed * FrameSpeed();
                    NPC? target = FindClosestEnemy(500);
                    if (target != null && CanHitNPCWithLineOfSight(target))
                    {
                        var shotPosition = Projectile.Center + Main.rand.NextVector2Circular(32, 32);
                        var targetPosition = target.Center + target.velocity;
                        var shotSpeed = 4f;
                        var targetDir = shotPosition.DirectionTo(targetPosition);
                        CreateChildProjectile<EelSplatanaSmallProjectile>(shotPosition, targetDir * shotSpeed, Projectile.damage * 2);
                    }
                }
            }

            // Visuals
            Projectile.direction = Math.Sign(Projectile.velocity.X);
            Projectile.rotation = MathHelper.Lerp(Projectile.rotation, Projectile.velocity.X * 0.1f, 0.1f);

            if (Main.rand.NextBool(3))
            {
                var d = Dust.NewDustPerfect(
                    Position: Projectile.Center + Vector2.UnitY * 10 + Main.rand.NextVector2Circular(20, 20),
                    Type: ModContent.DustType<ChargerBulletDust>(),
                    Velocity: WoomyMathHelper.AddRotationToVector2(-Vector2.UnitY * 3, -30, 30),
                    newColor: ColorHelper.AddRandomHue(30, Color.MediumPurple),
                    Scale: Main.rand.NextFloat(1f, 1.5f));
            }

            if (timeSpentAlive % 4 == 0)
            {
                var rect = new Rectangle(Projectile.Hitbox.Left - 30, Projectile.Hitbox.Top - 50, 80, 80);
                var sparkleColor = Main.rand.NextBool(2) ? Color.Orange : Color.MediumPurple;
                sparkleColor = ColorHelper.ColorWithAlpha255(sparkleColor);

                if (Main.rand.NextBool(5))
                {
                    var d = Dust.NewDustPerfect(
                        Position: Main.rand.NextVector2FromRectangle(rect),
                        Type: DustID.AncientLight,
                        Velocity: Vector2.Zero,
                        newColor: sparkleColor,
                        Scale: Main.rand.NextFloat(1f, 1.5f));

                    d.noGravity = true;
                }
            }

            // Audio
            if (Projectile.timeLeft <= timeLeftWhenFade * 2)
            {
                return;
            }

            if (timeSpentAlive % 30 == 0)
            {
                PlayAudio(SoundID.Item131, volume: 0.2f, pitchVariance: 0.3f, pitch: -3f, maxInstances: 10);
            }

            if (timeSpentAlive % 40 == 0 && Main.rand.NextBool(2))
            {
                PlayAudio(SoundID.Drown, volume: 0.2f, pitchVariance: 0.3f, pitch: 0.8f, maxInstances: 10);
            }

            if (timeSpentAlive % 20 == 0 && Main.rand.NextBool(5))
            {
                List<SoundStyle> sounds = new()
                {
                    SoundID.Item85,
                    SoundID.Item86,
                    SoundID.Item87,
                };

                PlayAudio(Main.rand.NextFromCollection(sounds), volume: 0.2f, pitchVariance: 0.3f, pitch: 0f, maxInstances: 10);
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            var size = 64;
            hitbox = new Rectangle((int)Projectile.Center.X - size / 2, (int)Projectile.Center.Y - size / 2, size, size);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (timeSpentAlive > 5)
            {
                if (fading)
                {
                    drawAlpha = Projectile.timeLeft / (float)timeLeftWhenFade;
                }

                float scale = drawScale;

                DrawProjectile(
                    Color.White,
                    rotation: Projectile.rotation,
                    scale: scale,
                    alphaMod: drawAlpha,
                    considerWorldLight: true,
                    positionOffset: new Vector2(0, -16),
                    flipSpriteSettings: (SpriteEffects)(Projectile.direction == 0 ? 0 : 1));
            }

            return false;
        }
        public override bool PreAI()
        {
            base.PreAI();
            previousPositionX = Projectile.position.X;
            previousPositionY = Projectile.position.Y;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}
