using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Prefixes.DualiePrefixes;

internal class BaseDualiePrefix : BaseWeaponPrefix
{
    public virtual int ExtraDodgeRolls => 0;

    public static LocalizedText ExtraDodgeRollsTooltip { get; private set; }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        ExtraDodgeRollsTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ExtraDodgeRollsTooltip)}");
    }

    public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
    {
        foreach (var line in base.GetTooltipLines(item))
        {
            yield return line;
        }

        if (ExtraDodgeRolls != 0)
        {
            yield return CreateTooltip(ExtraDodgeRollsTooltip, ExtraDodgeRolls, false);
        }
    }
}
