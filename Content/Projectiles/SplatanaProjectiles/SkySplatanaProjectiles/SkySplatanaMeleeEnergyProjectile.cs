using Terraria;
using Microsoft.Xna.Framework;
using AchiSplatoon2.Helpers;
using Terraria.ID;
using AchiSplatoon2.Content.EnumsAndConstants;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles.SkySplatanaProjectiles
{
    internal class SkySplatanaMeleeEnergyProjectile : SplatanaMeleeEnergyProjectile
    {
        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            SetSwingSettings(5, 20, 0.7f);

            var hueOffset = Main.rand.Next(-15, 15);
            var lightCol = ColorHelper.IncreaseHueBy(hueOffset, Color.LightSeaGreen);
            var darkCol = ColorHelper.IncreaseHueBy(hueOffset, Color.DodgerBlue);

            SetSwingVisuals(
                lightCol,
                darkCol,
                TexturePaths.Medium4pSparkle.ToTexture2D(),
                TexturePaths.Glow100x.ToTexture2D(),
                true);
        }

        protected override void SlashSound()
        {
            PlayAudio(SoundID.Item7, volume: 1f, pitchVariance: 0.3f, pitch: 0.5f, maxInstances: 10);
            PlayAudio(SoundID.Item18, volume: 0.8f, pitchVariance: 0f, pitch: 0.5f, maxInstances: 10);
            PlayAudio(SoundID.Item18, volume: 0.5f, pitchVariance: 0f, pitch: 1f, maxInstances: 10);

            PlayAudio(SoundID.DD2_DarkMageHealImpact, volume: 0.5f, pitchVariance: 0.3f, pitch: 1f, maxInstances: 10);
            PlayAudio(SoundID.DD2_BetsysWrathShot, volume: 0.2f, pitchVariance: 0f, pitch: 0.5f, maxInstances: 10);
        }

        protected override void CreateSwingDust(Vector2 position)
        {
            if (timeSpentAlive % FrameSpeedMultiply(2) == 0)
            {
                DustHelper.NewDust(
                    position + Main.rand.NextVector2Circular(40, 40),
                    DustID.SnowSpray,
                    WoomyMathHelper.AddRotationToVector2(GetDirectionWithOffset(), 90) * Main.rand.NextFloat(1, 6) * -initialDirection,
                    Color.White,
                    Main.rand.NextFloat(0.25f, 1f));
            }
        }

        protected override void HitTarget(NPC target)
        {
            var proj = CreateChildProjectile<HitProjectile>(target.Center, Vector2.Zero, Projectile.damage, true);
            proj.targetToHit = target.whoAmI;
            proj.immuneTime = 3;
        }

        protected override void CreateHitVisual(Vector2 position)
        {
            PlayAudio(SoundID.Item25, volume: 0.5f, pitchVariance: 0.3f, pitch: 0.5f, maxInstances: 1);

            for (int j = 0; j < 5; j++)
            {
                DustHelper.NewDust(
                    position,
                    DustID.SnowSpray,
                    Main.rand.NextVector2CircularEdge(5, 5),
                    Color.White,
                    Main.rand.NextFloat(1f, 2f));
            }

            for (int j = 0; j < 10; j++)
            {
                DustHelper.NewDust(
                    position,
                    DustID.HallowSpray,
                    position.DirectionTo(Owner.Center) * -Main.rand.NextFloat(2, 32) + Main.rand.NextVector2Circular(2, 2),
                    Color.White,
                    Main.rand.NextFloat(0.5f, 2f));
            }
        }
    }
}
