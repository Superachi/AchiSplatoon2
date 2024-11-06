using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Projectiles;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

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
                    projectile.position,
                    projectile.width,
                    projectile.height,
                    ModContent.DustType<ChargerBulletDust>(),
                    newColor: baseProjectile.initialColor,
                    Scale: Main.rand.NextFloat(1f, 1.4f));
                d.velocity = Main.rand.NextVector2CircularEdge(3, 3);

                d = Dust.NewDustDirect(
                    projectile.position,
                    projectile.width,
                    projectile.height,
                    ModContent.DustType<SplatterBulletLastingDust>(),
                    newColor: baseProjectile.initialColor,
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

        public static void ShooterSpawnVisual(BaseProjectile baseProjectile)
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
                    newColor: baseProjectile.initialColor,
                    Scale: Main.rand.NextFloat(1f, 1.4f));
                d.velocity = WoomyMathHelper.AddRotationToVector2(projectile.velocity, Main.rand.NextFloat(-30, 30)) * 1.5f;
            }
        }
    }
}
