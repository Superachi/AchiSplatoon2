using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Prefixes.ChargeWeaponPrefixes;

internal class BaseChargeWeaponPrefix : BaseWeaponPrefix
{
    public static LocalizedText ChargeSpeedTooltip { get; private set; }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        ChargeSpeedTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ChargeSpeedTooltip)}");
    }

    public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
    {
        foreach (var line in base.GetTooltipLines(item))
        {
            yield return line;
        }

        if (ChargeSpeedModifier != 0f)
        {
            yield return CreateTooltip(ChargeSpeedTooltip, ChargeSpeedModifier, false);
        }
    }
}
