using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Prefixes.BrushPrefixes
{
    internal class BaseBrushPrefix : BaseWeaponPrefix
    {
        public virtual float DashSpeedModifier => 0f;

        public static LocalizedText DashSpeedTooltip { get; private set; }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DashSpeedTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(DashSpeedTooltip)}");
        }

        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            foreach (var line in base.GetTooltipLines(item))
            {
                yield return line;
            }

            if (DashSpeedModifier != 0)
            {
                yield return CreateTooltip(DashSpeedTooltip, DashSpeedModifier, false);
            }
        }
    }
}
