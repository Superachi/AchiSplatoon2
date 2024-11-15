using AchiSplatoon2.Content.Prefixes.ChargeWeaponPrefixes;
using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Prefixes.StringerPrefixes;

internal class BaseStringerPrefix : BaseChargeWeaponPrefix
{
    public virtual float ShotgunArcModifier => 0f;

    public static LocalizedText ShotgunArcTooltip { get; private set; }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        ShotgunArcTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ShotgunArcTooltip)}");
    }

    public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
    {
        foreach (var line in base.GetTooltipLines(item))
        {
            yield return line;
        }

        if (ShotgunArcModifier != 0)
        {
            yield return CreateTooltip(ShotgunArcTooltip, ShotgunArcModifier, true);
        }
    }

    public override void ApplyProjectileStats(BaseProjectile projectile)
    {
        base.ApplyProjectileStats(projectile);

        if (projectile is TriStringerCharge stringerCharge)
        {
            if (ExtraProjectileBonus > 0)
            {
                if (stringerCharge.shotgunArc == 0) stringerCharge.shotgunArc = 2f;
                stringerCharge.shotgunArc += stringerCharge.shotgunArc * ExtraProjectileBonus;
            }

            stringerCharge.shotgunArc *= (1 + ShotgunArcModifier);
            stringerCharge.projectileCount += ExtraProjectileBonus;
            stringerCharge.burstRequiredHits += ExtraProjectileBonus;
        }
    }
}
