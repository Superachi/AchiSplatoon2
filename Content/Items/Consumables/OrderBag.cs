using AchiSplatoon2.Content.Items.Consumables.ColorVials;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Consumables
{
    internal class OrderBag : BaseItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
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
                SpawnOrderWeapons(player);
            }

            return true;
        }

        public override void RightClick(Player player)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                SpawnOrderWeapons(player);
            }
        }

        private void SpawnOrderWeapons(Player player)
        {
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<OrderShot>());
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<StarterSplatBomb>());
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<Inkzooka>());
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<ConfigVial>());
        }
    }
}
