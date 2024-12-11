using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Buffs;
using System;

namespace AchiSplatoon2.Content.Items.Accessories.Emblems
{
    internal class LastDitchEffortEmblem : BaseAccessory
    {
        public static float LifePercentageThreshold = 0.6f;
        public static float InkSaverAmount = 0.6f;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(
            (int)Math.Round(LifePercentageThreshold * 100),
            (int)Math.Round(InkSaverAmount * 100));

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 28;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                if ((float)player.statLife / (float)player.statLifeMax2 <= LifePercentageThreshold)
                {
                    player.AddBuff(ModContent.BuffType<LastDitchEffortBuff>(), 2);
                }
            }
        }
    }
}
