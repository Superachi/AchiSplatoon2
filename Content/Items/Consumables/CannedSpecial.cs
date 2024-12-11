using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Consumables
{
    internal class CannedSpecial : BaseItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IgnoresEncumberingStone[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 17;
            Item.useTime = Item.useAnimation;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.rare = ItemRarityID.Orange;
        }

        public override bool? UseItem(Player player)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var specialPlayer = player.GetModPlayer<SpecialPlayer>();
                specialPlayer.IncrementSpecialCharge(specialPlayer.SpecialPointsMax);

                return true;
            }

            return false;
        }

        public override bool OnPickup(Player player)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var specialPlayer = player.GetModPlayer<SpecialPlayer>();
                specialPlayer.IncrementSpecialCharge(specialPlayer.SpecialPointsMax);
            }

            return false;
        }
    }
}
