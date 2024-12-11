using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.DualieProjectiles
{
    internal class PainterDualieShotProjectile : DualieShotProjectile
    {
        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            UpdateCurrentColor(ColorHelper.LerpBetweenColorsPerfect(Main.DiscoColor, Color.White, 0.1f));
        }

        protected override void PlayShootSound()
        {
            var dualieMP = GetOwner().GetModPlayer<DualiePlayer>();
            if (dualieMP.isTurret)
            {
                PlayAudio(SoundID.NPCHit16, volume: 0.3f, pitchVariance: 0.2f, maxInstances: 5, pitch: 0.8f);
            }
            else
            {
                PlayAudio(SoundID.NPCHit16, volume: 0.4f, pitchVariance: 0.2f, maxInstances: 5, pitch: 0.5f);
            }
        }
    }
}
