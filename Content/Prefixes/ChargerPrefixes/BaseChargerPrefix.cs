using AchiSplatoon2.Content.Prefixes.ChargeWeaponPrefixes;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Prefixes.ChargerPrefixes;

internal class BaseChargerPrefix : BaseChargeWeaponPrefix
{
    public virtual bool LosePiercingModifier => false;
    public virtual bool ExplosiveModifier => false;

    public static LocalizedText LosePiercingTooltip { get; private set; }
    public static LocalizedText ExplosiveTooltip { get; private set; }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        LosePiercingTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(LosePiercingTooltip)}");
        ExplosiveTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ExplosiveTooltip)}");
    }

    public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
    {
        foreach (var line in base.GetTooltipLines(item))
        {
            yield return line;
        }

        if (LosePiercingModifier)
        {
            yield return CreateTooltip(LosePiercingTooltip, LosePiercingModifier, true, "Can't pierce enemies");
        }

        if (ExplosiveModifier)
        {
            yield return CreateTooltip(ExplosiveTooltip, ExplosiveModifier, false, "Charged shots explode");
        }
    }
}