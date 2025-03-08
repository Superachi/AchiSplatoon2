using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Prefixes.SlosherPrefixes
{
    internal class BaseSlosherPrefix : BaseWeaponPrefix
    {
        public virtual int RepetitionBonus => 0;
        public virtual int AmmoBonus => 0;

        public static LocalizedText RepetitionTooltip { get; private set; }
        public static LocalizedText AmmoTooltip { get; private set; }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            RepetitionTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(RepetitionTooltip)}");
            AmmoTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(AmmoTooltip)}");
        }

        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            foreach (var line in base.GetTooltipLines(item))
            {
                yield return line;
            }

            if (RepetitionBonus != 0)
            {
                yield return CreateTooltip(RepetitionTooltip, RepetitionBonus, false);
            }

            if (AmmoBonus != 0)
            {
                yield return CreateTooltip(AmmoTooltip, AmmoBonus, false);
            }
        }
    }
}
