using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Projectiles;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Helpers
{
    internal static class ProjectileDustHelper
    {
        public static void ShooterTileCollideVisual(BaseProjectile baseProjectile, bool playSound = true)
        {
            Projectile projectile = baseProjectile.Projectile;

            for (int i = 0; i < 10; i++)
            {
                var d = Dust.NewDustDirect(
                    projectile.Center,
                    1,
                    1,
                    ModContent.DustType<ChargerBulletDust>(),
                    newColor: baseProjectile.CurrentColor,
                    Scale: Main.rand.NextFloat(1f, 1.4f));
                d.velocity = Main.rand.NextVector2CircularEdge(3, 3);

                d = Dust.NewDustDirect(
                    projectile.Center,
                    1,
                    1,
                    ModContent.DustType<SplatterBulletLastingDust>(),
                    newColor: baseProjectile.CurrentColor,
                    Scale: Main.rand.NextFloat(0.8f, 1.2f));
                d.velocity = Main.rand.NextVector2Circular(3, 3);
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

            SoundHelper.PlayAudio(Main.rand.NextFromCollection(sounds), volume: 0.05f, pitchVariance: 0.5f, maxInstances: 50, pitch: 1f);
            SoundHelper.PlayAudio(SoundID.Splash, volume: 0.1f, pitchVariance: 0.5f, maxInstances: 50, pitch: 1f);
        }

        public static void ShooterSpawnVisual(BaseProjectile baseProjectile, float velocityMod = 1f)
        {
            Projectile projectile = baseProjectile.Projectile;

            for (int i = 0; i < 5; i++)
            {
                float random = Main.rand.NextFloat(-2, 2);
                var d = Dust.NewDustDirect(
                    projectile.position,
                    projectile.width,
                    projectile.height,
                    ModContent.DustType<ChargerBulletDust>(),
                    newColor: baseProjectile.CurrentColor,
                    Scale: Main.rand.NextFloat(1f, 1.4f));
                d.velocity = WoomyMathHelper.AddRotationToVector2(projectile.velocity, Main.rand.NextFloat(-30, 30)) * 1.5f * velocityMod;
            }
        }

        public static void BlasterSpawnVisual(BaseProjectile baseProjectile)
        {
            Projectile projectile = baseProjectile.Projectile;
            baseProjectile.UpdateCurrentColor(ColorHelper.LerpBetweenColorsPerfect(Color.White, baseProjectile.InitialColor, 0.5f));

            for (int i = 1; i < 20; i++)
            {
                var d = Dust.NewDustDirect(
                    projectile.Center,
                    projectile.width,
                    projectile.height,
                    DustID.AncientLight,
                    newColor: ColorHelper.ColorWithAlpha255(baseProjectile.InitialColor),
                    Scale: Main.rand.NextFloat(0.5f, 2f));
                d.velocity = WoomyMathHelper.AddRotationToVector2(projectile.velocity, Main.rand.NextFloat(-20, 20)) * Main.rand.NextFloat(0.25f, 0.5f) * i;
                d.noGravity = true;
            }
        }

        public static void BlasterDustTrail(BaseProjectile baseProjectile)
        {
            Projectile projectile = baseProjectile.Projectile;

            Dust d;

            if (baseProjectile.timeSpentAlive > 5)
            {
                baseProjectile.UpdateCurrentColor(ColorHelper.LerpBetweenColorsPerfect(baseProjectile.CurrentColor, baseProjectile.InitialColor, 0.1f));

                d = Dust.NewDustDirect(
                projectile.Center,
                0,
                0,
                ModContent.DustType<ChargerBulletDust>(),
                newColor: baseProjectile.CurrentColor,
                Scale: Main.rand.NextFloat(1.2f, 2f));

                d.velocity = Main.rand.NextVector2Circular(0.5f, 1);
            }

            if (baseProjectile.timeSpentAlive > 20 && Main.rand.NextBool(30))
            {
                d = Dust.NewDustPerfect(
                    Position: projectile.Center,
                    Type: DustID.FireworksRGB,
                    newColor: ColorHelper.IncreaseHueBy(Main.rand.Next(-20, 20), baseProjectile.CurrentColor),
                    Scale: Main.rand.NextFloat(0.8f, 1.2f));

                d.velocity = WoomyMathHelper.AddRotationToVector2(-projectile.velocity, -30, 30);
                d.noGravity = true;
            }
        }
    }
}
