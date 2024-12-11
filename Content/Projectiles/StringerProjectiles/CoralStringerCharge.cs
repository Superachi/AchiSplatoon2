using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class CoralStringerCharge : TriStringerCharge
    {
        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            SoundHelper.StopSoundIfActive(chargeStartAudio);
        }

        protected override void PlayShootSample()
        {
            var step = GetOwnerModPlayer<StatisticsPlayer>().attacksUsed;

            int[] pitchArray = new int[] {
                11, 11, 13, 11, 9,
                11, 11, 13, 11, 9,
                11, 11, 13, 11, 9,
                4, 6, 4, 6,
                11, 11, 13, 11, 9,

                11, 11, 13, 16, 9,
                11, 11, 11, 13, 11,
                9, 9, 9, 6, 4, 6
            };
            var pitch = (float)(pitchArray[step % pitchArray.Length] % 24);

            if (IsChargeMaxedOut())
            {
                PlayAudio(SoundID.Item26, volume: 0.5f, maxInstances: 10, pitch: 1f / 12);
                PlayAudio(SoundID.Item26, volume: 0.4f, maxInstances: 10, pitch: 6f / 12);
                PlayAudio(SoundID.Item26, volume: 0.3f, maxInstances: 10, pitch: 9f / 12);

                PlayAudio(SoundID.Drown, volume: 0.5f, maxInstances: 3, pitch: 1f);
                PlayAudio(SoundID.Item98, volume: 0.2f, maxInstances: 3, pitch: 0.3f);
            }
            else
            {
                PlayAudio(SoundID.Item26, volume: 0.5f, maxInstances: 10, pitch: (pitch - 12) / 12);
            }
        }

        protected override void SetChargeLevelModifiers(float chargePercentage)
        {
            switch (chargeLevel)
            {
                case 0:
                    projectileCount = 1;
                    Projectile.damage = (int)(Projectile.damage * 0.5);
                    velocityModifier = 0.6f;
                    break;
                case 1:
                    projectileCount /= 2;
                    Projectile.damage = (int)(Projectile.damage * 0.75);
                    velocityModifier = 0.8f;
                    finalArc /= 2;
                    break;
                case 2:
                    Projectile.penetrate += 3;
                    velocityModifier = 1f;
                    break;
            }

            if (projectileCount < 2)
            {
                finalArc = 0;
                return;
            }

            if (WeaponInstance is OrderStringer)
            {
                velocityModifier *= 0.75f;
            }

            finalArc = shotgunArc / (projectileCount / 2);
        }
    }
}
