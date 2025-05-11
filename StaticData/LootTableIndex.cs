using System.Collections.Generic;

namespace AchiSplatoon2.StaticData
{
    public class LootTableIndex
    {
        public List<int> itemIds { get; } = new();
        public int MinimumDropped { get; }
        public int MaximumDropped { get; }
        public int ChanceDenominator { get; }
        public int NpcId { get; } = -1;
        public int TreasureBagId { get; } = -1;

        // Single item
        public LootTableIndex(
            int itemId,
            int npcId,
            int minimumDropped = 1,
            int maximumDropped = 1,
            int chanceDenominator = 1,
            int treasureBagId = -1)
        {
            itemIds.Add(itemId);
            MinimumDropped = minimumDropped;
            MaximumDropped = maximumDropped;
            ChanceDenominator = chanceDenominator;
            NpcId = npcId;
            TreasureBagId = treasureBagId;
        }

        // Multiple items
        public LootTableIndex(
            int[] itemIdOptions,
            int npcId,
            int minimumDropped = 1,
            int maximumDropped = 1,
            int chanceDenominator = 1,
            int treasureBagId = -1
            )
        {
            foreach (var modItemId in itemIdOptions)
            {
                itemIds.Add(modItemId);
            }

            NpcId = npcId;
            MinimumDropped = minimumDropped;
            MaximumDropped = maximumDropped;
            TreasureBagId = treasureBagId;
            ChanceDenominator = chanceDenominator;
        }

        public static List<LootTableIndex> CreateLootTableIndicesSingleItem(
            int itemId,
            Dictionary<int, int> npcIdAndBagId,
            int minimumDropped = 1,
            int maximumDropped = 1,
            int chanceDenominator = 1)
        {
            var list = new List<LootTableIndex>();

            foreach (var pair in npcIdAndBagId)
            {
                var lootTableIndex = new LootTableIndex(
                    itemId: itemId,
                    minimumDropped: minimumDropped,
                    maximumDropped: maximumDropped,
                    chanceDenominator: chanceDenominator,
                    npcId: pair.Key,
                    treasureBagId: pair.Value
                );

                list.Add(lootTableIndex);
            }

            return list;
        }

        public static List<LootTableIndex> CreateLootTableIndicesMultipleItems(
            int[] itemIdOptions,
            Dictionary<int, int> npcIdAndBagId,
            int minimumDropped = 1,
            int maximumDropped = 1,
            int chanceDenominator = 1)
        {
            var list = new List<LootTableIndex>();

            foreach (var pair in npcIdAndBagId)
            {
                var lootTableIndex = new LootTableIndex(
                    itemIdOptions: itemIdOptions,
                    minimumDropped: minimumDropped,
                    maximumDropped: maximumDropped,
                    chanceDenominator: chanceDenominator,
                    npcId: pair.Key,
                    treasureBagId: pair.Value
                );

                list.Add(lootTableIndex);
            }

            return list;
        }
    }
}
