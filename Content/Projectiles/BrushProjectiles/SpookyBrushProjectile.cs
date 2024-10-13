using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Content.Dusts;
using Terraria.ID;

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
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
        }

        public override void AfterSpawn()
        {
            Initialize();
            startingVelocity = Projectile.velocity;
            currentColor = initialColor;

            SoundHelper.PlayAudio(SoundID.Item1, 0.5f, maxInstances: 10, pitch: 0.2f, position: Projectile.Center);

            var pitch = sineDirection == -1 ? 0.6f : 0.4f;
            SoundHelper.PlayAudio(SoundID.Item9, 0.2f, maxInstances: 10, pitch: pitch, position: Projectile.Center);
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
                float frequency = 5;
                float speedMod = 4;

                if (amplitude < 5)
                {
                    amplitude += 0.02f;
                }

                currentRadians = startingRadians + (float)Math.Sin(MathHelper.ToRadians(timeSpentAlive * frequency + 90) * sineDirection);
                Projectile.velocity = currentRadians.ToRotationVector2() * amplitude + Vector2.Normalize(startingVelocity) * speedMod;
            }

            currentColor = ColorHelper.IncreaseHueBy(0.25f, currentColor);

            if (timeSpentAlive > 4 * FrameSpeed())
            {
                visible = true;
            }

            if (visible)
            {
                Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: ModContent.DustType<SplatterBulletDust>(),
                    Velocity: Vector2.Zero,
                    newColor: currentColor,
                    Scale: 1.2f);

                if (Main.rand.NextBool(5))
                {
                    Dust dust = Dust.NewDustDirect(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.RainbowTorch,
                        newColor: currentColor,
                        Scale: Main.rand.NextFloat(0.5f, 1f)
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
            Color color = currentColor;
            float scale = 1 + (float)Math.Sin(MathHelper.ToRadians(timeSpentAlive * 2)) * 0.25f;

            Main.EntitySpriteDraw(shotSprite, position, null, color, drawRotation, origin, scale + 0.5f, SpriteEffects.None);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (sineCooldown == 0)
            {
                bouncesLeft--;
                SoundHelper.PlayAudio(SoundID.Item4, 0.3f, maxInstances: 10, pitch: 0.6f, pitchVariance: 0.2f, position: Projectile.Center);

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
    }
}
