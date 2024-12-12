using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Prefixes.BrellaPrefixes
{
    internal class BaseBrellaPrefix : BaseWeaponPrefix
    {
        public virtual float ShieldLifeModifier => 0f;
        public virtual float ShieldCooldownModifier => 0f;

        public static LocalizedText ShieldLifeTooltip { get; private set; }
        public static LocalizedText ShieldCooldownTooltip { get; private set; }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ShieldLifeTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ShieldLifeTooltip)}");
            ShieldCooldownTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ShieldCooldownTooltip)}");
        }

        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            foreach (var line in base.GetTooltipLines(item))
            {
                yield return line;
            }

            if (ShieldLifeModifier != 0)
            {
                yield return CreateTooltip(ShieldLifeTooltip, ShieldLifeModifier, false);
            }

            if (ShieldCooldownModifier != 0)
            {
                yield return CreateTooltip(ShieldCooldownTooltip, ShieldCooldownModifier, true);
            }
        }
    }
}
