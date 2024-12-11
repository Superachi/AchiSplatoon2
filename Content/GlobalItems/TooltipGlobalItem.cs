using AchiSplatoon2.Helpers;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.GlobalItems
{
    internal class TooltipGlobalItem : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.BlackInk)
            {
                var newTooltip = new TooltipLine(Mod, "Tooltip#1", ColorHelper.TextWithFunctionalColor("Used to make Black Dye, but also other items!"));
                tooltips[2] = newTooltip;
            }
        }
    }
}
