using AchiSplatoon2.Content.Projectiles;

namespace AchiSplatoon2.Content.Prefixes.ChargeWeaponPrefixes;

internal class BaseChargeWeaponPrefix : BaseWeaponPrefix
{
    public override void ApplyProjectileStats(BaseProjectile projectile)
    {
        base.ApplyProjectileStats(projectile);

        if (projectile is BaseChargeProjectile chargeProjectile)
        {
            chargeProjectile.PrefixChargeTimeMultiplier = (1 + ChargeSpeedModifier);
        }
    }
}
