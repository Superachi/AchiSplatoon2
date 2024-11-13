using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.Content.Prefixes.ChargeWeaponPrefixes;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;

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
