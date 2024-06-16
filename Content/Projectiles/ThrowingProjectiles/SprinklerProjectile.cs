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

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class SprinklerProjectile : BaseProjectile
    {
        private float delayUntilFall = 12f;
        private float fallSpeed = 0.3f;
        private float terminalVelocity = 12f;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Initialize();

            PlayAudio(SoundID.SplashWeak, volume: 0.5f, pitchVariance: 0.3f, maxInstances: 5, pitch: 2f);
            if (Main.rand.NextBool(2))
            {
                PlayAudio(SoundID.SplashWeak, volume: 0.4f, pitchVariance: 0.3f, maxInstances: 5);
            }
            if (Main.rand.NextBool(2))
            {
                PlayAudio(SoundID.Splash, volume: 0.2f, pitchVariance: 0.3f, maxInstances: 5, pitch: 2f);
            }

            var spreadOffset = 0.5f;
            Projectile.velocity.X += Main.rand.NextFloat(-spreadOffset, spreadOffset);
            Projectile.velocity.Y += Main.rand.NextFloat(-spreadOffset, spreadOffset);
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;

            // Start falling eventually
            if (Projectile.ai[0] >= delayUntilFall)
            {
                Projectile.velocity.Y += fallSpeed;

                if (Projectile.velocity.Y >= 0)
                {
                    Projectile.velocity.X *= 0.98f;
                }
            }

            if (Projectile.velocity.Y > terminalVelocity)
            {
                Projectile.velocity.Y = terminalVelocity;
            }

            Color dustColor = GenerateInkColor();
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Vector2.Zero, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
            for (int i = 0; i < 3; i++)
            {
                // Vector2 spawnPosition = Projectile.oldPosition != Vector2.Zero ? Vector2.Lerp(Projectile.position, Projectile.oldPosition, Main.rand.NextFloat()) : Projectile.position;
                var dust = Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Projectile.velocity / 5, newColor: dustColor, Scale: 1.2f);
                dust.alpha = 64;
            }
        }
    }
}
