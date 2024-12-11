using AchiSplatoon2.Helpers;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.BrellaProjectiles.CorruptionBrellaProjectiles
{
    internal class CorruptionBrellaShotgunProjectile : BrellaShotgunProjectile
    {
        protected override void PlayShootSound()
        {
            SoundHelper.PlayAudio(SoundID.DD2_BetsyFireballShot, 0.3f, pitchVariance: 0.4f, maxInstances: 5, pitch: 0f, position: Projectile.Center);
            SoundHelper.PlayAudio(SoundID.Item20, 0.5f, pitchVariance: 0.2f, maxInstances: 5, pitch: -0.5f, position: Projectile.Center);
        }
    }
}
