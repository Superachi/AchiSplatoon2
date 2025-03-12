using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles.SkySplatanaProjectiles
{
    internal class SkySplatanaMeleeEnergyProjectile : SplatanaMeleeEnergyProjectile
    {
        private bool _isUpgraded = false;

        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            SetSwingSettings(5, 20, 0.7f);

            _isUpgraded = (BaseSplatana)WeaponInstance is SkySplatanaB;

            Color lightCol, darkCol;
            if (!_isUpgraded)
            {
                var hueOffset = Main.rand.Next(-15, 15);
                lightCol = ColorHelper.IncreaseHueBy(hueOffset, Color.LightSeaGreen);
                darkCol = ColorHelper.IncreaseHueBy(hueOffset, Color.DodgerBlue);
            }
            else
            {
                var hueOffset = Main.rand.Next(-5, 5);
                lightCol = ColorHelper.IncreaseHueBy(hueOffset, new Color(255, 100, 255));
                darkCol = ColorHelper.IncreaseHueBy(hueOffset, new Color(130, 80, 255));
            }

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
                    WoomyMathHelper.AddRotationToVector2(GetDirectionWithOffset(), 90) * Main.rand.NextFloat(1, 6) * -swingDirection,
                    Color.White,
                    Main.rand.NextFloat(0.25f, 1f));
            }
        }

        protected override void HitTarget(NPC target)
        {
            base.HitTarget(target);
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
                    !_isUpgraded ? Color.Aqua : Color.Red,
                    Main.rand.NextFloat(1f, 2f));
            }

            for (int j = 0; j < 10; j++)
            {
                DustHelper.NewDust(
                    position,
                    !_isUpgraded ? DustID.HallowSpray : DustID.CrystalPulse,
                    position.DirectionTo(Owner.Center) * -Main.rand.NextFloat(2, 32) + Main.rand.NextVector2Circular(2, 2),
                    Color.White,
                    Main.rand.NextFloat(0.5f, 2f));
            }
        }
    }
}
