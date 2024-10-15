using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.BrellaProjectiles.MartianBrellaProjectiles
{
    internal class MartianBrellaShotgunProjectile : BrellaShotgunProjectile
    {
        protected override void PlayShootSound()
        {
            SoundHelper.PlayAudio(SoundID.Item67, 0.1f, pitchVariance: 0.4f, maxInstances: 5, pitch: 0.8f, position: Projectile.Center);
            SoundHelper.PlayAudio(SoundID.DD2_LightningBugZap, 0.6f, pitchVariance: 0.2f, maxInstances: 5, pitch: 0.6f, position: Projectile.Center);
        }
    }
}
