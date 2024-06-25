using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace AchiSplatoon2.Content.Projectiles.BrushProjectiles
{
    internal class InkbrushProjectile : BaseProjectile
    {
        private Color bulletColor;
        private float delayUntilFall;
        private float fallSpeed = 0.05f;
        private float airResist = 0.99f;
        private float terminalVelocity = 8f;
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
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Initialize();
            BaseBrush weaponData = (BaseBrush)weaponSource;
            shootSample = weaponData.ShootSample;
            shootAltSample = weaponData.ShootAltSample;
            delayUntilFall = weaponData.DelayUntilFall;

            // Set visuals
            Projectile.frame = Main.rand.Next(0, Main.projFrames[Projectile.type]);
            bulletColor = GenerateInkColor();
            drawRotation += MathHelper.ToRadians(Main.rand.Next(0, 359));
            drawScale += Main.rand.NextFloat(1f, 1.5f);

            // Play sound
            if (Main.rand.NextBool(2))
            {
                PlayAudio(shootSample, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 5);
            }
            else
            {
                PlayAudio(shootAltSample, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 5);
            }
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.position, bulletColor.R * brightness, bulletColor.G * brightness, bulletColor.B * brightness);

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

            if (Projectile.velocity.Y > terminalVelocity)
            {
                Projectile.velocity.Y = terminalVelocity;
            }

            // Spawn dust
            if (Timer % 3 == 0 && Main.rand.NextBool(2))
            {
                Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Projectile.velocity * 0.2f, newColor: GenerateInkColor(), Scale: Main.rand.NextFloat(1f, 1.5f));
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
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(Main.projFrames[Projectile.type], frameX: Projectile.frame); // The sourceRectangle says which frame to use.
            Vector2 origin = sourceRectangle.Size() / 2f;

            // The light value in the world
            var lightInWorld = Lighting.GetColor(Projectile.Center.ToTileCoordinates());

            // Keep the ink color (glowColor), but reduce its brightness if the environment is dark
            var finalColor = new Color(bulletColor.R * lightInWorld.R / 255, bulletColor.G * lightInWorld.G / 255, bulletColor.B * lightInWorld.G / 255);

            Main.EntitySpriteDraw(texture, position, sourceRectangle, finalColor, drawRotation, origin, drawScale, new SpriteEffects(), 0f);
            return false;
        }
    }
}
