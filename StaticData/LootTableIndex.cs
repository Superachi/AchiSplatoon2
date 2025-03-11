using System.Collections.Generic;

namespace AchiSplatoon2.StaticData
{
    public class LootTableIndex
    {
        public int ModItemId { get; }
        public int MinimumDropped { get; }
        public int MaximumDropped { get; }
        public int ChanceDenominator { get; }
        public int NpcId { get; } = -1;
        public int TreasureBagId { get; } = -1;

        public LootTableIndex(
            int modItemId,
            int npcId,
            int minimumDropped = 1,
            int maximumDropped = 1,
            int chanceDenominator = 1,
            int treasureBagId = -1)
        {
            ModItemId = modItemId;
            MinimumDropped = minimumDropped;
            MaximumDropped = maximumDropped;
            ChanceDenominator = chanceDenominator;
            NpcId = npcId;
            TreasureBagId = treasureBagId;
        }

        public static List<LootTableIndex> CreateLootTableIndices(
            int modItemId,
            Dictionary<int, int> npcIdAndBagId,
            int minimumDropped = 1,
            int maximumDropped = 1,
            int chanceDenominator = 1)
        {
            var list = new List<LootTableIndex>();

            foreach (var pair in npcIdAndBagId)
            {
                var lootTableIndex = new LootTableIndex(
                    modItemId: modItemId,
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
