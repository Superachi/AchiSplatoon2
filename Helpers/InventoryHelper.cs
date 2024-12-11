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
    }
}
