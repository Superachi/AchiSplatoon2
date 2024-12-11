using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Consumables.DroneUpgradeDiscs
{
    internal class DroneDiscBase : BaseItem
    {
        public override void SetDefaults()
        {
            Item.consumable = true;

            Item.width = 32;
            Item.height = 32;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 30;
            Item.useTime = Item.useAnimation;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item84;

            Item.maxStack = 1;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override bool CanUseItem(Player player)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<PearlDronePlayer>();
                return modPlayer.CheckIfDiscCanBeUsed(this);
            }

            return false;
        }

        public override bool? UseItem(Player player)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<PearlDronePlayer>();
                modPlayer.LevelUp(this);
                return true;
            }

            return false;
        }
    }
}
