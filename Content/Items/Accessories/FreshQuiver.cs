using AchiSplatoon2.Content.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria;
using AchiSplatoon2.Helpers;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Accessories
{
    internal class FreshQuiver : BaseAccessory
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(gold: 7, silver: 50);
            Item.rare = ItemRarityID.Pink;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<InkWeaponPlayer>();
                modPlayer.hasFreshQuiver = true;
            }
        }
    }
}
