using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.Helpers;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Items.Weapons.Dualies;

namespace AchiSplatoon2.Content.Prefixes.DualiePrefixes;

internal class BaseDualiePrefix : BaseWeaponPrefix
{
    public virtual int ExtraDodgeRolls => 0;

    public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
    {
        if (ExtraDodgeRolls != 0)
        {
            yield return new TooltipLine(Mod, "PrefixExtraDodgeRolls", ExtraDodgeRollsTooltip.Format(ExtraDodgeRolls))
            {
                IsModifier = true,
                IsModifierBad = false
            };
        }
    }

    public static LocalizedText ExtraDodgeRollsTooltip { get; private set; }

    public override void SetStaticDefaults()
    {
        ExtraDodgeRollsTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ExtraDodgeRollsTooltip)}");
    }
}
