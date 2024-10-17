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
            var weapons = new List<int>
            {
                ModContent.ItemType<OrderBrush>(),
                ModContent.ItemType<OrderCharger>(),
                ModContent.ItemType<OrderStringer>(),
                ModContent.ItemType<OrderRoller>(),
                ModContent.ItemType<OrderBlaster>(),
                ModContent.ItemType<OrderBrella>(),
            };

            var chips = new List<int>
            {
                ModContent.ItemType<ColorChipRed>(),
                ModContent.ItemType<ColorChipBlue>(),
                ModContent.ItemType<ColorChipYellow>(),
                ModContent.ItemType<ColorChipPurple>(),
                ModContent.ItemType<ColorChipGreen>()
            };

            int firstWeapon = Main.rand.NextFromCollection<int>(weapons);

            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<OrderShot>());
            player.QuickSpawnItem(player.GetSource_DropAsItem(), firstWeapon);
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<SplatBomb>(), 10);

            int chosenChip = Main.rand.NextFromCollection<int>(chips);
            player.QuickSpawnItem(player.GetSource_DropAsItem(), chosenChip);
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<ColorChipEmpty>());
            player.QuickSpawnItem(player.GetSource_DropAsItem(), ModContent.ItemType<ChipPalette>());
        }
    }
}
