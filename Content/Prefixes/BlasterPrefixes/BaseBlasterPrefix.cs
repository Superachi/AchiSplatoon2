using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.Content.Projectiles.BlasterProjectiles;
using AchiSplatoon2.ExtensionMethods;

namespace AchiSplatoon2.Content.Prefixes.BlasterPrefixes;

internal class BaseBlasterPrefix : BaseWeaponPrefix
{
    public override void ApplyProjectileStats(BaseProjectile projectile)
    {
        base.ApplyProjectileStats(projectile);

        if (projectile is BlasterProjectile blasterProjectile)
        {
            if (ExplosionRadiusModifier != 0)
            {
                blasterProjectile.ModifyExplosionRadius(ExplosionRadiusModifier.NormalizePrefixMod());
            }

            if (LifetimeModifier != 0)
            {
                blasterProjectile.ModifyExplosionDelay(LifetimeModifier.NormalizePrefixMod());
            }
        }
    }
}
