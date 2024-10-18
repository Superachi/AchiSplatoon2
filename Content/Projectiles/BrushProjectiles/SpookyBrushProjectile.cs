using AchiSplatoon2.Content.Dusts;
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
        private Color currentColor;
        private float drawRotation = 0;
        private bool visible = false;

        private int SineCooldownMax => 6 * FrameSpeed();

        public override void SetStaticDefaults()
        {
            if (shotSprite == null)
            {
                shotSprite = ModContent.Request<Texture2D>("AchiSplatoon2/Content/Assets/Textures/NebulaStringerShot").Value;
            }
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 3;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = true;
        }

        public override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            startingVelocity = Projectile.velocity;
            currentColor = initialColor;

            var pitch = sineDirection == -1 ? 0.6f : 0.4f;
            SoundHelper.PlayAudio(SoundID.NPCHit52, 0.3f, maxInstances: 10, pitch: pitch, position: Projectile.Center);
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
                float frequency = 5f;
                float speedMod = 3;

                if (amplitude < 4)
                {
                    amplitude += 0.05f;
                }

                currentRadians = startingRadians + (float)Math.Sin(MathHelper.ToRadians(timeSpentAlive * frequency + 90) * sineDirection);
                Projectile.velocity = currentRadians.ToRotationVector2() * amplitude + Vector2.Normalize(startingVelocity) * speedMod;
            }

            drawRotation += Projectile.velocity.Length() / 200f;
            currentColor = ColorHelper.IncreaseHueBy(0.25f, currentColor);

            if (timeSpentAlive > 5 * FrameSpeed())
            {
                visible = true;
            }

            if (visible)
            {
                Dust dust = Dust.NewDustDirect(
                    Position: Projectile.Center,
                    Width: 1,
                    Height: 1,
                    Type: ModContent.DustType<SplatterBulletLastingDust>(),
                    newColor: currentColor,
                    Scale: Main.rand.NextFloat(0.8f, 1.2f));
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(0.5f, 0.5f);

                if (Main.rand.NextBool(5))
                {
                    dust = Dust.NewDustDirect(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.RainbowTorch,
                        newColor: currentColor,
                        Scale: Main.rand.NextFloat(0.4f, 0.8f)
                    );
                    dust.noGravity = true;
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

            if (shotSprite == null)
            {
                shotSprite = ModContent.Request<Texture2D>("AchiSplatoon2/Content/Assets/Textures/NebulaStringerShot").Value;
            }

            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Vector2 origin = shotSprite.Size() / 2;
            float scale = 1 + (float)Math.Sin(MathHelper.ToRadians(timeSpentAlive * 2)) * 0.25f;

            Main.EntitySpriteDraw(shotSprite, position, null, currentColor, drawRotation, origin, scale + 0.6f, SpriteEffects.None);
            Main.EntitySpriteDraw(shotSprite, position, null, new Color(255, 255, 255, 0f), drawRotation, origin, scale + 0.4f, SpriteEffects.None);
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
        }

        public override void OnKill(int timeLeft)
        {
            if (timeLeft == 0)
            {
                DustBurstSparkle();
            }
        }

        private void DustBurst()
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustPerfect(
                Position: Projectile.Center,
                Type: ModContent.DustType<SplatterBulletLastingDust>(),
                Velocity: Main.rand.NextVector2CircularEdge(3, 3),
                newColor: currentColor,
                Scale: 0.8f);
            }
        }

        private void DustBurstSparkle()
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: DustID.AncientLight,
                    Velocity: Main.rand.NextVector2Circular(20, 20),
                    newColor: currentColor,
                    Scale: 1.2f);
                dust.noGravity = true;
            }
        }
    }
}
