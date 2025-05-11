using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.ProjectileVisuals;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.BrushProjectiles
{
    internal class SpookyBrushProjectile : BaseProjectile
    {
        private int bouncesLeft = 3;

        private Vector2 startingVelocity;
        public int sineDirection = 0;
        private float currentRadians;
        private float amplitude = 0;
        private int sineCooldown = 0;

        private Texture2D? shotSprite = null;
        private float drawRotation = 0;
        private bool visible = false;

        private int SineCooldownMax => 6 * FrameSpeed();

        public override void SetStaticDefaults()
        {
            if (shotSprite == null)
            {
                shotSprite = TexturePaths.NebulaStringerShot.ToTexture2D();
            }
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 9;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 60 * Projectile.extraUpdates;
            Projectile.tileCollide = true;
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            startingVelocity = Projectile.velocity;

            var pitch = sineDirection == -1 ? 0.6f : 0.4f;
            SoundHelper.PlayAudio(SoundID.DD2_GoblinBomberThrow, 0.2f, maxInstances: 10, pitch: pitch, position: Projectile.Center);
            SoundHelper.PlayAudio(SoundID.NPCHit52, 0.1f, maxInstances: 10, pitch: pitch, position: Projectile.Center);
        }

        public override void AI()
        {
            if (sineCooldown > 0)
            {
                sineCooldown--;
            }

            if (sineCooldown == 0)
            {
                float startingRadians = startingVelocity.ToRotation();
                float frequency = 2f;
                float speedMod = 0.5f;
                amplitude = 1f;

                currentRadians = startingRadians + (float)Math.Sin(MathHelper.ToRadians(timeSpentAlive * frequency + 90) * sineDirection);
                Projectile.velocity = currentRadians.ToRotationVector2() * amplitude + Vector2.Normalize(startingVelocity) * speedMod;
            }

            drawRotation += Projectile.velocity.Length() / 200f;
            UpdateCurrentColor(ColorHelper.IncreaseHueBy(0.25f, CurrentColor));

            if (timeSpentAlive > 5 * FrameSpeed() && !visible)
            {
                visible = true;

                var sparkle = CreateSparkle(Projectile.Center);
                sparkle.AdjustScale(0.8f);

                SoundHelper.PlayAudio(SoundID.Item25, 0.1f, maxInstances: 10, pitch: 0.4f, pitchVariance: 0.3f, position: Projectile.Center);
                SoundHelper.PlayAudio(SoundID.Item28, 0.1f, maxInstances: 10, pitch: 0.6f, pitchVariance: 0.3f, position: Projectile.Center);
                SoundHelper.PlayAudio(SoundID.Item66, 0.1f, maxInstances: 10, pitch: 0.8f, position: Projectile.Center);
            }

            if (visible)
            {
                var d = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: DustID.PortalBolt,
                    Velocity: Vector2.Zero,
                    newColor: CurrentColor,
                    Scale: 1f);
                d.noGravity = true;
                d.noLight = true;

                if (Main.rand.NextBool(4))
                {
                    d = Dust.NewDustPerfect(
                        Position: Projectile.Center,
                        Type: DustID.PortalBolt,
                        Velocity: Main.rand.NextVector2Circular(2, 2),
                        newColor: CurrentColor,
                        Scale: 1f);
                    d.noGravity = true;
                    d.noLight = true;
                }
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            SetHitboxSize(20, out hitbox);
        }

        public override void PostDraw(Color lightColor)
        {
            if (!visible) return;

            // Draw layered sparkle + glow
            SpriteBatch spriteBatch = Main.spriteBatch;

            var sparkleSprite = TexturePaths.Medium4pSparkle.ToTexture2D();
            var glowSprite = TexturePaths.Glow100x.ToTexture2D();

            float scale = 0.5f + (float)Math.Sin(MathHelper.ToRadians(timeSpentAlive * 5)) / 8;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(
                texture: sparkleSprite,
                position: Projectile.Center - Main.screenPosition,
                sourceRectangle: null,
                color: ColorHelper.ColorWithAlpha255(Color.White),
                rotation: 0,
                origin: sparkleSprite.Size() / 2,
                scale: scale * 0.6f,
                effects: SpriteEffects.None);

            Main.EntitySpriteDraw(
                texture: sparkleSprite,
                position: Projectile.Center - Main.screenPosition,
                sourceRectangle: null,
                color: ColorHelper.ColorWithAlpha255(CurrentColor),
                rotation: 0,
                origin: sparkleSprite.Size() / 2,
                scale: scale * 1.2f,
                effects: SpriteEffects.None);

            Main.EntitySpriteDraw(
                texture: glowSprite,
                position: Projectile.Center - Main.screenPosition,
                sourceRectangle: null,
                color: ColorHelper.ColorWithAlpha255(CurrentColor) * 0.5f,
                rotation: 0,
                origin: glowSprite.Size() / 2,
                scale: scale * 1.5f,
                effects: SpriteEffects.None);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (sineCooldown == 0)
            {
                Projectile.damage = MultiplyProjectileDamage(0.7f);
                bouncesLeft--;
                SoundHelper.PlayAudio(SoundID.Item115, 0.2f, maxInstances: 10, pitch: 0.7f, pitchVariance: 0.3f, position: Projectile.Center);
                DustBurst();
            }

            if (bouncesLeft > 0)
            {
                ProjectileBounce(oldVelocity, new Vector2(0.8f, 0.8f));
                sineCooldown = SineCooldownMax;
                amplitude = 0;
                startingVelocity = Projectile.velocity;
                sineDirection *= -1;

                return false;
            }

            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            DustBurstSparkle();
            CreateSparkle(Projectile.Center);
        }

        protected override void AfterKill(int timeLeft)
        {
            if (timeLeft == 0)
            {
                DustBurstSparkle();
                CreateSparkle(Projectile.Center);
            }
        }

        private void DustBurst()
        {
            for (int i = 0; i < 10; i++)
            {
                var d = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: DustID.AncientLight,
                    Velocity: Main.rand.NextVector2CircularEdge(3, 3),
                    newColor: CurrentColor,
                    Scale: 0.8f);
                d.noGravity = true;
            }
        }

        private void DustBurstSparkle()
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: DustID.AncientLight,
                    Velocity: Main.rand.NextVector2Circular(15, 15),
                    newColor: CurrentColor,
                    Scale: 1.2f);
                dust.noGravity = true;
            }
        }

        private StillSparkleVisual CreateSparkle(Vector2 position)
        {
            var sparkle = CreateChildProjectile<StillSparkleVisual>(position, Vector2.Zero, 0, true);
            sparkle.UpdateCurrentColor(CurrentColor);
            sparkle.AdjustRotation(0);
            return sparkle;
        }
    }
}
