using AchiSplatoon2.Content.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace AchiSplatoon2.Helpers
{
    internal static class ProjectileDustHelper
    {
        public static void ShooterTileCollideVisual(BaseProjectile baseProjectile, bool playSound = true)
        {
            Projectile projectile = baseProjectile.Projectile;

            for (int i = 0; i < 10; i++)
            {
                DustHelper.NewSplatterBulletDust(
                    position: projectile.Center,
                    velocity: Main.rand.NextVector2Circular(3, 3),
                    color: baseProjectile.CurrentColor,
                    minScale: 0.8f,
                    maxScale: 1.4f);

                DustHelper.NewLastingDust(
                    position: projectile.Center,
                    velocity: Main.rand.NextVector2Circular(4, 4),
                    color: baseProjectile.CurrentColor,
                    minScale: 1f,
                    maxScale: 1.4f);
            }

            List<SoundStyle> sounds = new()
            {
                SoundID.NPCHit8,
                SoundID.NPCHit13,
                SoundID.NPCHit17,
                SoundID.NPCHit19,
                SoundID.NPCHit29
            };

            if (!playSound) return;

            SoundHelper.PlayAudio(Main.rand.NextFromCollection(sounds), volume: 0.1f, pitchVariance: 0.5f, maxInstances: 50, pitch: 1f, position: projectile.position);
            SoundHelper.PlayAudio(SoundID.Splash, volume: 0.2f, pitchVariance: 0.5f, maxInstances: 50, pitch: 1f, position: projectile.position);
        }

        public static void ShooterSpawnVisual(BaseProjectile baseProjectile, float velocityMod = 1f)
        {
            Projectile projectile = baseProjectile.Projectile;

            for (int i = 0; i < 5; i++)
            {
                var d = DustHelper.NewChargerBulletDust(
                    position: projectile.position,
                    velocity: WoomyMathHelper.AddRotationToVector2(projectile.velocity, Main.rand.NextFloat(-30, 30)) * 1.5f * velocityMod,
                    color: baseProjectile.CurrentColor,
                    minScale: 1f,
                    maxScale: 1.4f);
            }
        }

        public static void BlasterSpawnVisual(BaseProjectile baseProjectile)
        {
            Projectile projectile = baseProjectile.Projectile;
            baseProjectile.UpdateCurrentColor(ColorHelper.LerpBetweenColorsPerfect(Color.White, baseProjectile.InitialColor, 0.5f));

            for (int i = 1; i < 20; i++)
            {
                var d = DustHelper.NewDust(
                    position: projectile.Center,
                    dustType: DustID.AncientLight,
                    velocity: WoomyMathHelper.AddRotationToVector2(projectile.velocity, Main.rand.NextFloat(-20, 20)) * Main.rand.NextFloat(0.25f, 0.5f) * i,
                    color: ColorHelper.ColorWithAlpha255(baseProjectile.InitialColor),
                    scale: Main.rand.NextFloat(0.5f, 2f));
            }
        }

        public static void BlasterDustTrail(BaseProjectile baseProjectile)
        {
            Projectile projectile = baseProjectile.Projectile;

            Dust d;

            if (baseProjectile.timeSpentAlive > 5)
            {
                baseProjectile.UpdateCurrentColor(ColorHelper.LerpBetweenColorsPerfect(baseProjectile.CurrentColor, baseProjectile.InitialColor, 0.1f));

                d = DustHelper.NewChargerBulletDust(
                    position: projectile.Center,
                    velocity: Main.rand.NextVector2Circular(1, 1),
                    color: baseProjectile.CurrentColor,
                    minScale: 1.2f,
                    maxScale: 2f);
            }

            if (baseProjectile.timeSpentAlive > 20 && Main.rand.NextBool(30))
            {
                d = DustHelper.NewDust(
                    position: projectile.Center,
                    dustType: DustID.FireworksRGB,
                    velocity: WoomyMathHelper.AddRotationToVector2(-projectile.velocity, -30, 30),
                    color: baseProjectile.CurrentColor,
                    scale: Main.rand.NextFloat(0.8f, 1.2f));
            }
        }
    }
}
