using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Helpers
{
    internal class InventoryHelper
    {
        public static Item? FirstInInventory<T>(Player player, out int inventoryIndex)
            where T : ModItem
        {
            inventoryIndex = 0;

            for (int i = 0; i < player.inventory.Length; i++)
            {
                inventoryIndex = i;
                var item = player.inventory[i];

                if (item.IsAir)
                {
                    continue;
                }

                if (item.ModItem is T)
                {
                    return item;
                }
            }

            return null;
        }

        public static Item? FirstInInventory<T>(Player player)
            where T : ModItem
        {
            return FirstInInventory<T>(player, out int _);
        }

        public static Item? FirstInInventoryRange<T>(Player player, int startInvSlot, int endInvSlot)
            where T : ModItem
        {
            if (startInvSlot >= player.inventory.Length
                || startInvSlot < 0
                || endInvSlot >= player.inventory.Length
                || endInvSlot < 0
                || startInvSlot >= endInvSlot)
            {
                DebugHelper.PrintError($"Tried to look for an item in the inventory, but the specified range where the search takes place is invalid (range: {startInvSlot}, {endInvSlot}).");
                return null;
            }

            for (int i = startInvSlot; i < endInvSlot; i++)
            {
                var item = player.inventory[i];

                if (item.IsAir)
                {
                    continue;
                }

                if (item.ModItem is T)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
