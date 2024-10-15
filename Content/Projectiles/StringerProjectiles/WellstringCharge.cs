using Terraria;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class WellstringCharge : TriStringerCharge
    {
        protected override void SetChargeLevelModifiers(float chargePercentage)
        {
            finalArc = shotgunArc / chargePercentage / (projectileCount / 2);

            if (chargeLevel == 0)
            {
                velocityModifier = 0.6f;
                Projectile.damage /= 3;
            }
            else
            {
                finalArc = shotgunArc / (1.1f - chargePercentage) / (projectileCount / 2);

                if (chargeLevel == 1)
                {
                    Projectile.damage = (int)(Projectile.damage * 0.5);
                    velocityModifier = 0.8f;
                }
                else
                {
                    Projectile.damage = (int)(Projectile.damage * 1);
                    velocityModifier = 1.2f;
                }
            }
        }
    }
}
