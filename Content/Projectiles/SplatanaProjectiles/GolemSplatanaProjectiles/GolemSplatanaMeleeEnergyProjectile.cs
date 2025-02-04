using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles.GolemSplatanaProjectiles
{
    internal class GolemSplatanaMeleeEnergyProjectile : SplatanaMeleeEnergyProjectile
    {
        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            SetSwingSettings(8, 20, 0.6f);

            var random = Main.rand.NextFloat(0f, 1f);
            var lightCol = ColorHelper.LerpBetweenColors(Color.Yellow, Color.LightYellow, 0.25f + random * 0.1f);
            var darkCol = ColorHelper.LerpBetweenColors(Color.DarkRed, Color.Red, random);

            SetSwingVisuals(
                lightCol,
                darkCol,
                TexturePaths.Medium8pSparkle.ToTexture2D(),
                TexturePaths.Glow100x.ToTexture2D(),
                true);
        }

        protected override void SlashSound()
        {
            PlayAudio(SoundID.Item7, volume: 1f, pitchVariance: 0.3f, pitch: 0f, maxInstances: 10);
            PlayAudio(SoundID.Item18, volume: 1f, pitchVariance: 0f, pitch: 0f, maxInstances: 10);

            PlayAudio(SoundID.Item66, volume: 0.5f, pitchVariance: 0f, pitch: 0f, maxInstances: 10);
            PlayAudio(SoundID.DD2_BetsysWrathShot, volume: 0.1f, pitchVariance: 0f, pitch: 0f, maxInstances: 10);
            PlayAudio(SoundID.DD2_PhantomPhoenixShot, volume: 1f, pitchVariance: 0f, pitch: 0.5f, maxInstances: 10);
            PlayAudio(SoundID.DD2_DrakinShot, volume: 0.3f, pitchVariance: 0f, pitch: 0.5f, maxInstances: 10);
        }

        protected override void CreateSwingDust(Vector2 position)
        {
            if (timeSpentAlive % FrameSpeedMultiply(2) == 0)
            {
                DustHelper.NewDust(
                    position + Main.rand.NextVector2Circular(40, 40),
                    DustID.Torch,
                    WoomyMathHelper.AddRotationToVector2(GetDirectionWithOffset(), 90) * Main.rand.NextFloat(1, 6) * -swingDirection,
                    Main.rand.Next([Color.Purple, Color.Red, Color.Orange]),
                    Main.rand.NextFloat(0.5f, 3f));
            }

            if (Main.rand.NextBool(10))
            {
                var d = DustHelper.NewDust(
                    position + Main.rand.NextVector2Circular(40, 40),
                    DustID.Smoke,
                    WoomyMathHelper.AddRotationToVector2(GetDirectionWithOffset(), 90) * Main.rand.NextFloat(1, 3) * -swingDirection,
                    ColorHelper.LerpBetweenColors(Color.Black, Color.Gray, 0.3f),
                    Main.rand.NextFloat(0.5f, 1f));

                d.noGravity = false;
            }
        }

        protected override void HitTarget(NPC target)
        {
            base.HitTarget(target);

            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.OnFire3, 300);
            }
        }

        protected override void CreateHitVisual(Vector2 position)
        {
            PlayAudio(SoundID.Item14, volume: 0.5f, pitchVariance: 0.2f, pitch: -0.5f, maxInstances: 5);

            for (int j = 0; j < 15; j++)
            {
                DustHelper.NewDust(
                    position,
                    DustID.Torch,
                    Main.rand.NextVector2Circular(8, 8),
                    ColorHelper.LerpBetweenColors(Color.Red, Color.Orange, Main.rand.NextFloat(0f, 1f)),
                    Main.rand.NextFloat(3f, 5f));
            }

            for (int j = 0; j < 3; j++)
            {
                var g = Gore.NewGoreDirect(
                    Terraria.Entity.GetSource_None(),
                    position,
                    Vector2.Zero,
                    GoreID.Smoke1,
                    Main.rand.NextFloat(1f, 2f));

                g.velocity = Main.rand.NextVector2Circular(3, 3);
                g.alpha = 128;
            }
        }
    }
}
