using AchiSplatoon2.Content.Prefixes.ChargeWeaponPrefixes;
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
}
