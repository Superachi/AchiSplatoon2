using AchiSplatoon2.Content.Prefixes.ChargeWeaponPrefixes;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Prefixes.SplatlingPrefixes
{
    internal class BaseSplatlingPrefix : BaseChargeWeaponPrefix
    {
        public virtual float ShotsPerChargeModifier => 0f;
        public virtual int ShotTimeModifier => 0;
        public virtual float ShotSpreadModifier => 0f;

        public static LocalizedText ShotsPerChargeTooltip { get; private set; }
        public static LocalizedText ShotTimeTooltip { get; private set; }
        public static LocalizedText ShotSpreadTooltip { get; private set; }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            ShotsPerChargeTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ShotsPerChargeTooltip)}");
            ShotTimeTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ShotTimeTooltip)}");
            ShotSpreadTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ShotSpreadTooltip)}");
        }

        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            foreach (var line in base.GetTooltipLines(item))
            {
                yield return line;
            }

            if (ShotsPerChargeModifier != 0)
            {
                yield return CreateTooltip(ShotsPerChargeTooltip, ShotsPerChargeModifier, false);
            }

            if (ShotTimeModifier != 0)
            {
                yield return CreateTooltip(ShotTimeTooltip, ShotTimeModifier, true);
            }

            if (ShotSpreadModifier != 0)
            {
                yield return CreateTooltip(ShotSpreadTooltip, ShotSpreadModifier, true);
            }
        }
    }
}
