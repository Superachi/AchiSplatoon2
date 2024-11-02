using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Consumables.LootBags
{
    [Autoload(false)]
    internal class BaseLootBag : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                OpenLootBag(player);
            }

            return false;
        }

        public override void RightClick(Player player)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                OpenLootBag(player);
            }
        }

        protected virtual void OpenLootBag(Player player)
        {

        }
    }
}
