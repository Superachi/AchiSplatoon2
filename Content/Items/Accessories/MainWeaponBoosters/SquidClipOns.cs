using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters
{
    internal class SquidClipOns : BaseWeaponBoosterAccessory
    {
        public static int ExtraMaxRolls = 1;
        public static float RollCooldownMult = 3f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ExtraMaxRolls);

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 30;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<InkAccessoryPlayer>();
                modPlayer.hasSquidClipOns = true;
            }
        }
    }
}
