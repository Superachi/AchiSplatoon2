using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Helpers
{
    internal static class TextHelper
    {
        public static string ItemEmoji<T>(bool withName = false) where T : ModItem
        {
            return ItemEmoji(ModContent.ItemType<T>(), withName);
        }

        public static string ItemEmoji(int type, bool withName = false)
        {
            bool isVanilla = ContentSamples.ItemsByType.TryGetValue(type, out var item);

            var itemName = "";
            if (!isVanilla)
            {
                ModItem? modItem = ModContent.GetModItem(type);

                if (modItem == null)
                {
                    throw new Exception("ItemEmoji: couldn't find the provided (modded) item type");
                }

                itemName = modItem.Name;
            }
            else
            {
                if (item == null)
                {
                    throw new Exception("ItemEmoji: couldn't find the provided (vanilla) item type");
                }

                itemName = item.Name;
            }

            if (withName)
            {
                return $"{itemName} [i:{type}]";
            }

            return $"[i:{type}]";
        }
    }
}
