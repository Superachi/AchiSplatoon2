using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Consumables.LootBags
{
    [Autoload(true)]
    internal class CorruptLootBag : LargeMimicLootBag
    {
        protected override void OpenLootBag(Player player)
        {
            base.OpenLootBag(player);

            var accessories = new List<int>
            {
                ModContent.ItemType<TentacularOcular>(),
                ModContent.ItemType<AdamantiteCoil>(),
                ModContent.ItemType<FieryPaintCan>(),
                ModContent.ItemType<MarinatedNecklace>(),
            };

            int chosenAccessory = Main.rand.NextFromCollection(accessories);
            player.QuickSpawnItem(player.GetSource_DropAsItem(), chosenAccessory);
        }
    }
}
