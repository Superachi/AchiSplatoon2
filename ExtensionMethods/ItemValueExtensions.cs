using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.ExtensionMethods
{
    internal static class ItemValueExtensions
    {
        /// <summary>
        /// Pre evil-bosses
        /// </summary>
        /// <param name="baseItem"></param>
        internal static void SetValuePreEvilBosses(this Item item)
        {
            item.rare = ItemRarityID.Blue;
            item.value = Item.sellPrice(gold: 2);
        }

        /// <summary>
        /// Post evil-bosses
        /// </summary>
        /// <param name="baseItem"></param>
        internal static void SetValuePostEvilBosses(this Item item)
        {
            item.rare = ItemRarityID.Green;
            item.value = Item.sellPrice(gold: 2, silver: 50);
        }

        /// <summary>
        /// Items just before Wall of Flesh
        /// </summary>
        /// <param name="baseItem"></param>
        internal static void SetValueLatePreHardmode(this Item item)
        {
            item.rare = ItemRarityID.Orange;
            item.value = Item.sellPrice(gold: 3);
        }

        /// <summary>
        /// For Cobalt/Palladium items
        /// </summary>
        /// <param name="baseItem"></param>
        internal static void SetValueLowHardmodeOre(this Item item)
        {
            item.rare = ItemRarityID.Orange;
            item.value = Item.sellPrice(gold: 3, silver: 50);
        }

        /// <summary>
        /// For Mythril/Orichalcum items
        /// </summary>
        /// <param name="baseItem"></param>
        internal static void SetValueMidHardmodeOre(this Item item)
        {
            item.rare = ItemRarityID.LightRed;
            item.value = Item.sellPrice(gold: 4);
        }

        /// <summary>
        /// For Adamantite/Titanium items
        /// </summary>
        /// <param name="baseItem"></param>
        internal static void SetValueHighHardmodeOre(this Item item)
        {
            item.rare = ItemRarityID.Pink;
            item.value = Item.sellPrice(gold: 5);
        }

        /// <summary>
        /// For Hallowed/Chlorophyte items
        /// </summary>
        /// <param name="baseItem"></param>
        internal static void SetValuePostMech(this Item item)
        {
            item.rare = ItemRarityID.LightPurple;
            item.value = Item.sellPrice(gold: 6);
        }

        /// <summary>
        /// For items dropping from Duke Fishron, Empress of Light, etc.
        /// </summary>
        /// <param name="baseItem"></param>
        internal static void SetValuePostPlantera(this Item item)
        {
            item.rare = ItemRarityID.Lime;
            item.value = Item.sellPrice(gold: 8);
        }

        /// <summary>
        /// For very special lategame drops
        /// </summary>
        /// <param name="baseItem"></param>
        internal static void SetValueEndgame(this Item item)
        {
            item.rare = ItemRarityID.Yellow;
            item.value = Item.sellPrice(gold: 10);
        }

        /// <summary>
        /// Vanity items
        /// </summary>
        /// <param name="baseItem"></param>
        internal static void SetValueVanity(this Item item)
        {
            item.rare = ItemRarityID.Blue;
            item.value = Item.sellPrice(silver: 10);
        }

        /// <summary>
        /// Potion items
        /// </summary>
        /// <param name="baseItem"></param>
        internal static void SetValuePotion(this Item item)
        {
            item.rare = ItemRarityID.Blue;
            item.value = Item.sellPrice(silver: 2, copper: 50);
        }

        /// <summary>
        /// Crafting materials
        /// </summary>
        /// <param name="baseItem"></param>
        internal static void SetValueCraftingMaterial(this Item item)
        {
            item.rare = ItemRarityID.Blue;
            item.value = Item.sellPrice(silver: 1);
        }
    }
}
