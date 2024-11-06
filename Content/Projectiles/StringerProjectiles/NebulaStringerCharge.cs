using AchiSplatoon2.Content.Players;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class NebulaStringerCharge : TriStringerCharge
    {
        protected override void SetChargeLevelModifiers(float chargePercentage)
        {
            finalArc = shotgunArc / (chargePercentage) / (projectileCount / 2);

            switch (chargeLevel)
            {
                case 0:
                    Projectile.damage /= 3;
                    velocityModifier = 4f;
                    break;
                case 1:
                    Projectile.damage = (int)(Projectile.damage * 0.5f);
                    velocityModifier = 5f;
                    finalArc /= 3;
                    break;
                case 2:
                    finalArc = shotgunArc / (1.1f - chargePercentage) / (projectileCount / 2);
                    velocityModifier = 3f;
                    break;
            }
        }

        protected override void SetChildProjectileProperties(BaseProjectile projectile, int projectileNumber = 0)
        {
            if (!IsProjectileOfType<NebulaStringerProjectile>(projectile)) return;

            var mP = GetOwner().GetModPlayer<AccessoryPlayer>();
            var modProj = projectile as NebulaStringerProjectile;
            modProj.parentFullyCharged = IsChargeMaxedOut();
            modProj.firedWithFreshQuiver = mP.hasFreshQuiver;

            projectile.Projectile.ai[2] = projectileNumber;
            projectile.AfterSpawn();
        }

        protected override void PlayShootSample()
        {
            switch (chargeLevel)
            {
                case 0:
                    PlayAudio(SoundID.Item158, volume: 0.3f, maxInstances: 3, pitch: 1);
                    break;
                case 1:
                    PlayAudio(SoundID.Item158, volume: 0.5f, maxInstances: 3, pitch: 0);
                    break;
                case 2:
                    PlayAudio(SoundID.Item9, volume: 0.3f, maxInstances: 3, pitch: 0);
                    PlayAudio(SoundID.Item43, volume: 0.7f, maxInstances: 3, pitch: 0);
                    break;
            }
        }
    }
}
