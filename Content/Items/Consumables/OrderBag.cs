using AchiSplatoon2.Content.Items.Accessories.ColorChips;
using AchiSplatoon2.Content.Items.Accessories.Palettes;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Rollers;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Helpers;
using System.Collections.Generic;
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

            return false;
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
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<SplatBomb>(), 10);

            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<ChipPalette>());
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<ColorChipAqua>());
        }
    }
}
